using Common;
using Common.Extensions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace WorkerQueue_Consumer
{
    class Program
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _model;

        private const string QueueName = "WorkerQueue_Example";

        public static void Main()
        {
            CreateQueue();
            Receive();
            Console.ReadLine();

        }

        internal static void CreateQueue()
        {
            _factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = _factory.CreateConnection();
            _model = _connection.CreateModel();

            _model.QueueDeclare(QueueName, true, false, false, null);
            _model.BasicQos(0, 1, false);
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
            _model.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);
        }
    }
}

