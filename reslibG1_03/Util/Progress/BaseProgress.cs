using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reslibG1_03.Util.Progress
{
    public class BaseProgress : IEquatable<BaseProgress>
    {
        private long _TotalSize;
        private long _CurrentSize;

        private ProgressResult _Result;
        private TimeSpan _ElapsedTime;
        private TimeSpan _EstimatedTimeRemaining;

        private long _ProcessingSpeed;

        public long TotalSize { get => _TotalSize; set => _TotalSize = value; }
        public long CurrentSize { get => _CurrentSize; set => _CurrentSize = value; }
        public short Percent { get => TotalSize > 0 && CurrentSize <= TotalSize ? (short)(Math.Round((double)(CurrentSize / TotalSize)) * 100) : 0; }

        public ProgressResult Result { get => _Result; set => _Result = value; }

        public TimeSpan ElapsedTime { get => _ElapsedTime; set => _ElapsedTime = value; }
        public TimeSpan RemainingTime { get => _EstimatedTimeRemaining; set => _EstimatedTimeRemaining = value; }

        public long ProcessingSpeed { get => _ProcessingSpeed; set => _ProcessingSpeed = value; }
        public long AverageSpeedPerSecond { get => _CurrentSize > 0 && ElapsedTime.TotalMilliseconds > 0 ? (long)(_CurrentSize / (ElapsedTime.TotalMilliseconds / 1000)) : 0; }


        public override bool Equals(object obj)
        {
            return Equals(obj as BaseProgress);
        }

        public bool Equals(BaseProgress prg)
        {
            if (prg is not null)
            {
                if (prg.TotalSize == this.TotalSize &&
                    prg.CurrentSize == this.CurrentSize &&
                    prg.ElapsedTime == this.ElapsedTime &&
                    prg.RemainingTime == this.RemainingTime &&
                    prg.ProcessingSpeed == this.ProcessingSpeed &&
                    prg.Result == this.Result)

                    return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 23 * 49;
                hash = (hash * 37) + TotalSize.GetHashCode();
                hash = (hash * 37) + CurrentSize.GetHashCode();
                hash = (hash * 37) + Result.GetHashCode();
                hash = (hash * 37) + ProcessingSpeed.GetHashCode();
                hash = (hash * 37) + ElapsedTime.GetHashCode();
                hash = (hash * 37) + RemainingTime.GetHashCode();

                return hash;
            }
        }
    }

    public enum ProgressResult { Completed, Processing, Error, Failed }
}
