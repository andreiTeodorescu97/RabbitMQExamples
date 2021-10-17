using System;
using static WorkerQueue_Producer.QueueOperations;

namespace WorkerQueue_Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("PROGRAM START!");

            var data = CreatePopulationData(50);

            CreateQueue();

            SendAllMessages(data);

            Console.WriteLine("PROGRAM END!");

            Console.ReadLine();
        }
    }
}
