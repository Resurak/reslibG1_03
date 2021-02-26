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
            Log.Info("starting streamTest");

            streamTest();

            Console.WriteLine("done");
            Console.ReadKey();
        }

        static void streamTest()
        {

        }
    }
}
