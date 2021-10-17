using Common;
using Common.Extensions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace PS_Publisher
{
    internal static class QueueOperations
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _model;

        private const string ExchangeName = "Exchange_Example";

        internal static IList<Payment> CreatePopulationData(int nrOfMembers)
        {
            var result = new List<Payment>();

            var initialName = "Payer";
            var rand = new Random();


            for (int i = 0; i < nrOfMembers; i++)
            {
                result.Add(new Payment
                {
                    AmountToPay = rand.NextDecimal(),
                    CardNumber = rand.Next(1000000000, 2147483647).ToString(),
                    Name = initialName + i.ToString()
                });
            }

            return result;
        }

        internal static void SendAllMessages(IList<Payment> payments)
        {
            foreach (var item in payments)
            {
                SendMesssage(item);
            }
        }

        private static int NextInt32(this Random rng)
        {
            int firstBits = rng.Next(0, 1 << 4) << 28;
            int lastBits = rng.Next(0, 1 << 28);
            return firstBits | lastBits;
        }

        private static decimal NextDecimal(this Random rng)
        {
            byte scale = (byte)rng.Next(29);
            bool sign = false;
            return new decimal(rng.NextInt32(),
                               rng.NextInt32(),
                               rng.NextInt32(),
                               sign,
                               scale);
        }

        internal static void CreateExchange()
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
        }

        internal static void SendMesssage(Payment message)
        {
            _model.BasicPublish(ExchangeName, "", null, message.Serialize());

            Console.WriteLine($"{DateTime.Now.ToString("HH:mm ddMMMyyyy")} " +
                $"Payment message sent: {message.CardNumber} : {message.AmountToPay} : {message.Name}");
        }
    }
}
