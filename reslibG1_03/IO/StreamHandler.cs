using reslibG1_03.Logging.Internal;
using reslibG1_03.Util.Progress;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reslibG1_03.IO
{
    public class StreamHandler : ProgressHandler
    {
        public bool ReportProgress { get; set; } = true;
        public int BufferSize { get; set; } = 1024 * 1024 *64;
        public long InputSize { get; set; } = -1;


        public void WriteInToOut(Stream s1, Stream s2)
        {
            if (s1 is null)
                throw new ArgumentNullException("Stream1 is null");

            if (s2 is null)
                throw new ArgumentNullException("Stream2 is null");

            long size = -1;

            if (s1.CanSeek | InputSize < 0)
                size = s1.Length;

            if (ReportProgress)
                StartProgress(size);

            WriteInternal(s1, s2);
        }


        private protected void WriteInternal(Stream s1, Stream s2)
        {
            bool completed = false;

            try
            {
                int read = 0;
                var buffer = new byte[BufferSize];

                while ((read = s1.Read(buffer, 0, BufferSize)) > 0)
                {
                    if (ReportProgress)
                        Progress.CurrentSize += read;

                    s2.Write(buffer, 0, BufferSize);
                }

                completed = true;
            }
            catch (Exception e)
            {
                InternalLogger.Exception("Writing streams failed", e);
                throw;
            }
            finally
            {
                if (completed)
                    OnCompletion();
                else
                    OnFailed();
            }
        }
    }
}
