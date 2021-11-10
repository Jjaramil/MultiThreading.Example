using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace MultiThreading.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            MultiThreadPrimeSearch();
            NormalPrimeSearch();
        }
        private static void NormalPrimeSearch()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.WriteLine("Started single threading");
            for (int i = 0; i < 20; i++)
            {
                FindPrimeNumber(i*192342);
            }
            stopwatch.Stop();
            Console.WriteLine("Elapsed Time for normal single thread is {0} ms", stopwatch.ElapsedMilliseconds);
        }
        private static void MultiThreadPrimeSearch()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.WriteLine("Started multithreading");
            int toProcess = 20;
            using (ManualResetEvent resetEvent = new ManualResetEvent(false))
            {
                var list = new List<int>();
                for(int i=0;i<20;i++) list.Add(i); 
                for (int i = 0; i < 20; i++)
                {
                    // Provides posibility of usage existing threads from ThreadPool instead of direcly creation a new Thread instance
                    ThreadPool.QueueUserWorkItem(new WaitCallback(x => {
                        FindPrimeNumber(i * 192342 );
                        if (Interlocked.Decrement(ref toProcess) == 0)
                            resetEvent.Set();
                    }), list[i]);
                }
                resetEvent.WaitOne();
            }
            stopwatch.Stop();
            Console.WriteLine("Elapsed Time for multithread is {0} ms", stopwatch.ElapsedMilliseconds);
        }

        public static long FindPrimeNumber(int n)
        {
            int count=0;
            long a = 2;
            while(count<n)
            {
                long b = 2;
                int prime = 1;
                while(b * b <= a)
                {
                    if(a % b == 0)
                    {
                        prime = 0;
                        break;
                    }
                    b++;
                }
                if(prime > 0)
                {
                    count++;
                }
                a++;
            }
            return (--a);
        }
    }
}