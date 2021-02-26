using reslibG1_03.IO;
using reslibG1_03.Logging;
using reslibG1_03.Util.Progress;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            Log.SetConfig(null, true);
            Log.Info("starting streamTest");

            streamTest();

            Console.WriteLine("done");
            Console.ReadKey();
        }

        static void streamTest()
        {
            var sw = new Stopwatch();
            sw.Start();

            //var file1 = @"C:\Users\Daniele\Desktop\Windows.iso";
            //var file2 = @"C:\Users\Daniele\Desktop\Windows copied.iso";

            //var file1 = @"C:\Users\Daniele\Desktop\test.mp4";
            //var file2 = @"C:\Users\Daniele\Desktop\test copied.mp4";

            var file1 = @"D:\sc11960-NMSC.part1.rar";
            var file2 = @"D:\sc11960-NMSC.part1 copied.rar";
            var file3 = @"D:\sc11960-NMSC.part1 copied 2.rar";

            using (var s1 = new FileStream(file2, FileMode.Open, FileAccess.Read, FileShare.Read, 512 * 1024, FileOptions.SequentialScan))
            using (var s2 = new FileStream(file3, FileMode.Create, FileAccess.Write, FileShare.Write, 512 * 1024, FileOptions.SequentialScan))
            using (var handler = new StreamHandler())
            {
                handler.ProgressChanged += (sender, e) => Console.WriteLine("Speed: " + LongToString(e.ProcessingSpeed) + "/s. Remaining: " + e.RemainingTime.ToString(@"hh\:mm\:ss"));
                handler.Completed += (sender, e) =>
                {
                    Console.WriteLine("Completed, elapsed: " + e.ElapsedTime.ToString(@"hh\:mm\:ss"));
                    Console.WriteLine("Average: " + LongToString(e.AverageSpeedPerSecond) + "/s");

                    Log.Info("file copied in " + e.ElapsedTime.TotalMilliseconds + "ms");
                    Log.Info("average speed: " + LongToString(e.AverageSpeedPerSecond));
                };

                handler.WriteInToOut(s1, s2);
            }

            sw.Stop();
            Console.WriteLine("total time: " + sw.ElapsedMilliseconds + "ms");
        }

        public static string LongToString(long totalByte, int decimals = 2)
        {
            double kB = 1024.00;
            double MB = 1048576.00;
            double GB = 1073741824.00;

            if (totalByte >= kB && totalByte < MB)
                return $"{Math.Round(totalByte / kB, decimals)} kB";
            else if (totalByte >= MB && totalByte < GB)
                return $"{Math.Round(totalByte / MB, decimals)} MB";
            else if (totalByte >= GB)
                return $"{Math.Round(totalByte / GB, decimals)} GB";
            else return $"{Math.Round((double)totalByte, 2)} B";
        }
    }
}
