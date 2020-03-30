using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TheIssueOfConcurrency
{
    public class Worker
    {
        public Queue<int> numbers = new Queue<int>();

        private object token = new object();

        public void CountToFive(int number)
        {
            Console.WriteLine($"CountToFive called: thread #{Thread.CurrentThread.ManagedThreadId}");

            //lock (token)
            //{

            Monitor.Enter(token);

            try
            {
                Console.WriteLine($"Got the token: thread #{Thread.CurrentThread.ManagedThreadId}");
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine($"{number} (thread #{Thread.CurrentThread.ManagedThreadId})");
                    numbers.Enqueue(number);
                    Thread.Sleep(100);
                    number++;
                }
                Console.WriteLine($"Returning the token: thread #{Thread.CurrentThread.ManagedThreadId}");
            }
            finally
            {
                Monitor.Exit(token);
            }
            //}
        }
    }

    class Program
    {
        private delegate void WorkerOp(int start);

        private static Worker worker;

        private static int operationsToFinish;

        static void Main(string[] args)
        {
            worker = new Worker();

            WorkerOp workerOp = worker.CountToFive;

            operationsToFinish = 2;
            workerOp.BeginInvoke(10, OnOperationFinished, null);
            workerOp.BeginInvoke(100, OnOperationFinished, null);

            // 10, 11, 12, 13, 14, 100, 101, 102, 103, 104
            // 100, 101, 102, 103, 104, 10, 11, 12, 13, 14

            // Wann sind beide delegates fertig?

            Console.ReadLine();
        }

        private static void OnOperationFinished(IAsyncResult ar)
        {
            Console.WriteLine("OnOperationFinished: thread #" + Thread.CurrentThread.ManagedThreadId);

            Interlocked.Decrement(ref operationsToFinish);
            if (operationsToFinish == 0)
            {
                foreach (int number in worker.numbers)
                {
                    Console.Write(number + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
