using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace reslibG1_03.Util.Progress
{
    public class ProgressHandler : IDisposable, IBaseProgressReporter
    {
        private Timer internalTimer;
        private short updateInterval;
        private long precedent;

        internal BaseProgress Progress;

        public event BaseProgressEventHandler ProgressChanged;
        public event BaseProgressEventHandler Completed;
        public event BaseProgressEventHandler Failed;

        private long total { get => Progress.TotalSize; set => Progress.TotalSize = value; }
        private long current { get => Progress.CurrentSize; set => Progress.CurrentSize = value; }
        private long processingSpeed { get => Progress.ProcessingSpeed; set => Progress.ProcessingSpeed = value; }

        private TimeSpan ElapsedTime { get => Progress.ElapsedTime; set => Progress.ElapsedTime = value; }
        private TimeSpan RemainingTime { get => Progress.RemainingTime; set => Progress.RemainingTime = value; }


        internal void StartProgress() => StartProgress(-1, -1);

        internal void StartProgress(long size) => StartProgress(size, -1);


        internal void StartProgress(long size, short interval)
        {
            Progress = new();
            Progress.Result = ProgressResult.Processing;

            total = size > 0 ? size : 0;
            current = 0;
            updateInterval = interval > 0 ? interval : 1000;

            if (internalTimer is not null)
                internalTimer.Dispose();

            internalTimer = new(updateInterval);
            internalTimer.Elapsed += TimerInternal;
            internalTimer.AutoReset = true;

            internalTimer.Start();
        }


        private protected void TimerInternal(object sender, ElapsedEventArgs elapsed)
        {
            processingSpeed = current - precedent;

            if (processingSpeed < 0)
                processingSpeed = 0;

            ElapsedTime = TimeSpan.FromMilliseconds(ElapsedTime.TotalMilliseconds + updateInterval);

            if (total > 0 && current > 0 && processingSpeed > 0)
                RemainingTime = TimeSpan.FromSeconds(Math.Round((double)((total - current) / processingSpeed)));

            precedent = current;

            OnUpdate();
        }


        internal void StopProgress(bool failed)
        {
            internalTimer.Stop();

            if (total > 0)
                current = total;

            RemainingTime = TimeSpan.FromSeconds(0);
            Progress.Result = failed ? ProgressResult.Failed : ProgressResult.Completed;

            if (failed)
                OnFailed();
            else
                OnCompletion();
        }


        internal void OnUpdate()
        {
            ProgressChanged?.Invoke(this, Progress);
        }


        internal void OnCompletion()
        {
            Completed?.Invoke(this, Progress);
        }


        internal void OnFailed()
        {
            Failed?.Invoke(this, Progress);
        }


        public void Dispose()
        {
            internalTimer.Elapsed -= TimerInternal;

            internalTimer.Close();
            internalTimer.Dispose();

            internalTimer = null;
        }
    }
}
