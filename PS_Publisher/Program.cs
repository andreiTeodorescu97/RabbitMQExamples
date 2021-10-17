using System;
using static PS_Publisher.QueueOperations;

namespace PS_Publisher
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("PROGRAM START!");

            var data = CreatePopulationData(50);

            CreateExchange();

            SendAllMessages(data);

            Console.WriteLine("PROGRAM END!");

            Console.ReadLine();
        }
    }
}
