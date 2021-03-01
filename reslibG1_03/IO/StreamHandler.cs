using reslibG1_03.Logging.Internal;
using reslibG1_03.Util.Common;
using reslibG1_03.Util.Progress;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reslibG1_03.IO
{
    public class StreamHandler : ProgressHandler, IAbortable
    {
        public bool ReportProgress { get; set; } = true;
        public int BufferSize { get; set; } = 1024 * 64;
        public long InputSize { get; set; } = -1;

        internal bool Writing = false;
        internal bool RequestAbort = false;

        internal event EventHandler Aborted;


        public void WriteInToOut(Stream s1, Stream s2)
        {
            if (s1 is null)
                throw new ArgumentNullException("Stream1 is null");

            if (s2 is null)
                throw new ArgumentNullException("Stream2 is null");

            long size = InputSize;

            if (s1.CanSeek && size < 0)
                size = s1.Length;

            if (ReportProgress)
                StartProgress(size);

            WriteInternal(s1, s2);
        }


        private protected void WriteInternal(Stream s1, Stream s2)
        {
            if (RequestAbort)
                throw new InvalidOperationException("Can't start writing streams with an aborted instance");
            
            bool completed = false;

            try
            {
                int read = 0;
                var buffer = new byte[BufferSize];

                Writing = true;

                while ((read = s1.Read(buffer, 0, BufferSize)) > 0)
                {
                    if (RequestAbort)
                        break;

                    if (ReportProgress)
                        Progress.CurrentSize += read;

                    s2.Write(buffer, 0, BufferSize);
                }

                completed = true;
            }
            catch (Exception e)
            {
                DebugLog.Exception("Writing streams failed", e);
                throw;
            }
            finally
            {
                Writing = false;

                if (ReportProgress && !RequestAbort)
                    if (completed)
                        OnCompletion();
                    else
                        OnFailed();
            }
        }

        public void Abort()
        {
            RequestAbort = true;

            while (Writing) { }

            Aborted?.Invoke(this, EventArgs.Empty);
        }
    }
}
