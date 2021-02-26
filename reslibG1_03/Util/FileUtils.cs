using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reslibG1_03.Util
{
    public static class FileUtils
    {
        public static void DeleteFile(string file)
        {
            if (File.Exists(file))
                File.Delete(file);
        }
    }
}
