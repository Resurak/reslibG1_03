using reslibG1_03.Util.Extensions;
using reslibG1_03.Util.Progress;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using reslibG1_03.Logging;

namespace reslibG1_03.IO
{
    public class ThreadedStreamHandler : ProgressHandler
    {
        public static object _Lock = new object();

        public void MT_Stream(Stream s1, Stream s2, short partsNum)
        {
            Log.Info("starting multi threaded file copy");

            var total = s1.Length;
            var parts = total.GetParts(partsNum);

            var threads = new Thread[partsNum];
            var streamsIn = new Stream[partsNum];
            var streamsOut = new Stream[partsNum];

            Log.Values(parts, "parts");
            Log.Info("offset: " + parts[0]);

            Log.Info("starting");
            StartProgress(total);

            s2.SetLength(total);
            s2.Seek(0, SeekOrigin.Begin);

            try
            {
                for (int i = 0; i < partsNum; i++)
                {
                    if (i >= partsNum)
                        break;

                    Log.Info("i = " + i);
                    Log.Info("starting thread with params: offset = " + parts[0] * i + "; partLenght = " + parts[i]);

                    streamsIn[i] = s1;
                    threads[i] = new Thread(() => Write(streamsIn[i], s2, parts[0] * i, parts[i]));
                    threads[i].Start();
                }
            }
            catch (Exception e)
            {
                Log.Exception("something went wrong in writing", e);
                throw;
            }
            //foreach (var t in threads)
            //    t.Start();

            for (int i = 0; i < partsNum; i++)
                threads[i].Join();

            OnCompletion();
        }

        private void Write(Stream s1, Stream s2, long offset, long partLenght)
        {
            var buffer = new byte[1024 * 64];
            long bytesRead = 0;
            long totalRead = 0;

            Log.Info("started thread #" + Thread.CurrentThread.ManagedThreadId);
            Log.Info("setting seek from #" + Thread.CurrentThread.ManagedThreadId);
            s1.Seek(offset, SeekOrigin.Begin);
            //s2.Seek(offset, SeekOrigin.Begin);

            while ((bytesRead = s1.Read(buffer, 0, buffer.Length)) > 0)
            {
                totalRead += bytesRead;

                Log.Info("writing from #" + Thread.CurrentThread.ManagedThreadId);

                lock(_Lock)
                {
                    s2.Seek(offset + totalRead, SeekOrigin.Begin);

                    if (totalRead >= partLenght)
                        s2.Write(buffer, 0, (int)(totalRead - partLenght));
                    else
                        s2.Write(buffer, 0, buffer.Length);

                    Progress.CurrentSize += bytesRead;
                }
            }

        }
    }
}
