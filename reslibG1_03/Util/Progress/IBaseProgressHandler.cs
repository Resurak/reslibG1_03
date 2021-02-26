using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reslibG1_03.Util.Progress
{
    public interface IBaseProgressHandler : IBaseProgressReporter
    {
        public ProgressHandler Handler { get; set; }
    }
}
