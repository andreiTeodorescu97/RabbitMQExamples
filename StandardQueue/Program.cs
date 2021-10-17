using System;
using static StandardQueue.QueueOperations;

namespace StandardQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("PROGRAM START!");

            var data = CreatePopulationData(10);

            CreateQueue();

            SendAllMessages(data);

            Receive();

            Console.WriteLine("PROGRAM END!");

            Console.ReadLine();
        }
    }
}
