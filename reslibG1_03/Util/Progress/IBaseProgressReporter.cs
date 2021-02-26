using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reslibG1_03.Util.Progress
{
    public delegate void BaseProgressEventHandler(object sender, BaseProgress e);

    public interface IBaseProgressReporter
    {
        public event BaseProgressEventHandler ProgressChanged;
        public event BaseProgressEventHandler Completed;
        public event BaseProgressEventHandler Failed;
    }
}
