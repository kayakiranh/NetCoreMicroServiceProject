using MP.Core.Application.DataTransferObjects;
using MP.Core.Application.Repositories;
using MP.Core.Domain.Enums;
using MP.Infrastructure.Helper;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP.Infrastructure.Queue.RabbitMq.Consumer
{
    public class CreditCardApplicationConsumer : ICreditCardApplicationConsumer
    {
        private readonly ILoggerRepository _loggerRepository;
        private readonly IMailerRepository _mailerRepository;
        private readonly IPostgreSqlRepository _postgreSqlRepository;
        public CreditCardApplicationConsumer(ILoggerRepository loggerRepository, IMailerRepository mailerRepository, IPostgreSqlRepository postgreSqlRepository)
        {
            _loggerRepository = loggerRepository;
            _mailerRepository = mailerRepository;
            _postgreSqlRepository = postgreSqlRepository;
        }

        public void ReadApplication()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
            };

            using (IConnection connection = connectionFactory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "creditcardapplication_queue",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        try
                        {
                            byte[] queueBody = ea.Body.ToArray();
                            string data = Encoding.UTF8.GetString(queueBody);
                            CreditCardApplicationDto cardApplicationDto = JsonConvert.DeserializeObject<CreditCardApplicationDto>(data);
                            PdfGenerateHelper.GeneratePDF(cardApplicationDto.CustomerId, cardApplicationDto.CreditCardId);
                            _mailerRepository.SendToAdmin(EmailTemplates.FinancialApiError, cardApplicationDto);
                            _postgreSqlRepository.Insert(cardApplicationDto);
                            _loggerRepository.Insert(LogTypes.Information, "Rabbitmq Success");
                        }
                        catch (Exception ex)
                        {
                            _loggerRepository.Insert(LogTypes.Critical, "Rabbitmq Error", ex);
                        }
                    };
                    channel.BasicConsume(queue: "creditcardapplication_queue", autoAck: true, consumer: consumer);
                }
            }
        }
    }
}