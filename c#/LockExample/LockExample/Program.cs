using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LockExample
{
    public class Worker
    {
        private object token = new object();

        private static Random random = new Random();

        public void PrintHello()
        {
            int randomWait = random.Next(200);

            Thread.Sleep(randomWait);

            lock (token)
            {
                Console.Write($"Thread #{Thread.CurrentThread.ManagedThreadId} says: Hello and ... ");
                Thread.Sleep(200);
                PrintGoodBye();
            }
        }

        public void PrintGoodBye()
        {
            lock (token)
            {
                Console.WriteLine($"Goodbye. [{Thread.CurrentThread.ManagedThreadId}]");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Worker worker = new Worker();

            Thread[] thread = new Thread[10];
            for (int i=0; i < 10; i++)
            {
                thread[i] = new Thread(worker.PrintHello);
                thread[i].Start();
            }

            Console.ReadLine();
        }
    }
}
