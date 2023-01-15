using MP.Core.Application.DataTransferObjects;
using MP.Core.Application.Repositories;
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
                        byte[] queueBody = ea.Body.ToArray();
                        string data = Encoding.UTF8.GetString(queueBody);
                        CreditCardApplicationDto cardApplicationDto = JsonConvert.DeserializeObject<CreditCardApplicationDto>(data);
                        //pdf oluştur
                        //mail gönder
                        //db update et

                    };
                    channel.BasicConsume(queue: "creditcardapplication_queue",
                                         autoAck: true,
                                         consumer: consumer);
                }
            }
        }
    }
}