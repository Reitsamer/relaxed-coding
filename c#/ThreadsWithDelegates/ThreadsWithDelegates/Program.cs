using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadsWithDelegates
{
    class Program
    {
        public delegate int BinaryOp(int x, int y);

        private static bool isDone = false;

        private static int Add(int a, int b)
        {
            Console.WriteLine($"Add() executed on thread #{Thread.CurrentThread.ManagedThreadId}");

            Thread.Sleep(4000);
            return a + b;
        }

        static void Main(string[] args)
        {
            Console.WriteLine($"Main() executed on thread #{Thread.CurrentThread.ManagedThreadId}");

            BinaryOp op = Add;

            //int result = op.Invoke(10, 10);

            // Thread thread = new Thread(Add);
            IAsyncResult asyncResult = op.BeginInvoke(10, 10, DelegateFinished, null);

            //while (!asyncResult.IsCompleted)
            //{
            //    Console.Write(".");
            //    Thread.Sleep(200);
            //}
            //Console.WriteLine();

            //while (!asyncResult.AsyncWaitHandle.WaitOne(20000, true))
            //{
            //    Console.Write(".");
            //}
            //Console.WriteLine();

            while (!isDone)
            {
                Console.Write(".");
                Thread.Sleep(200);
            }
            Console.WriteLine();

            // thread.Join();
            int result = op.EndInvoke(asyncResult);
            Console.WriteLine($"The result of 10 + 10 is: {result} (thread #{Thread.CurrentThread.ManagedThreadId})");

            Console.ReadLine();
        }

        private static void DelegateFinished(IAsyncResult ar)
        {
            Console.WriteLine($"Binary Operation Finished: thread #{Thread.CurrentThread.ManagedThreadId}");
            isDone = true;
        }
    }
}
