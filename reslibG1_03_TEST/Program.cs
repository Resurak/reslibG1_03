using reslibG1_03.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace reslibG1_03_TEST
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.StartLogging();
            Log.Info("Starting baby");
            Log.Message("com'è, biellu vieru??");

            Thread t1 = new(DoWork);
            Thread t2 = new(DoWork2);
            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            Console.WriteLine("done");

            //Console.ReadKey();
        }

        static void DoWork()
        {
            int i = 0;

            while (i < 1000)
            {
                Log.Info("log from another thread. thread id: " + Thread.CurrentThread.ManagedThreadId);
                i++;
            }
        }

        static void DoWork2()
        {
            int i = 0;

            while (i < 1000)
            {
                Log.Message("log from another thread. thread id: " + Thread.CurrentThread.ManagedThreadId);
                i++;
            }
        }
    }
}
