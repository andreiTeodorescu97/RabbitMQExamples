using Common;
using Common.Extensions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace PS_Subscriber
{
    class Program
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _model;
        private static string _queueName;

        private const string ExchangeName = "Exchange_Example";

        public static void Main()
        {
            CreateQueueAndConnectToExchange();
            Receive();
            Console.ReadLine();

        }

        internal static void CreateQueueAndConnectToExchange()
        {
            _factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = _factory.CreateConnection();
            _model = _connection.CreateModel();

            _model.ExchangeDeclare(ExchangeName, "fanout");

            _queueName = _model.QueueDeclare().QueueName;
            _model.QueueBind(_queueName, ExchangeName, "");

        }

        internal static void Receive()
        {
            var consumer = new EventingBasicConsumer(_model);
            consumer.Received += (sender, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);

                int dots = message.Split('.').Length - 1;
                Thread.Sleep(dots * 1000);

                Console.WriteLine(" [x] Done");

                // Note: it is possible to access the channel via
                //       ((EventingBasicConsumer)sender).Model here
                _model.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };
            _model.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
        }
    }
}
