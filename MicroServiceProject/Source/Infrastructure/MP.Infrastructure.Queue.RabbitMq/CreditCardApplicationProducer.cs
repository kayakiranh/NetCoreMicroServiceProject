using MP.Core.Application.DataTransferObjects;
using MP.Core.Application.Repositories;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace MP.Infrastructure.Queue.RabbitMq.Producer
{
    public class CreditCardApplicationProducer : ICreditCardApplicationProducer
    {
        public void SendApplication(CreditCardApplicationDto creditCardApplication)
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


                    string data = JsonConvert.SerializeObject(creditCardApplication);
                    byte[] queueData = Encoding.UTF8.GetBytes(data);

                    channel.BasicPublish(exchange: "",
                                         routingKey: "creditcardapplication_queue",
                                         basicProperties: null,
                                         body: queueData);
                }
            }
        }
    }
}