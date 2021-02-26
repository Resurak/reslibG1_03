using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace reslibG1_03.Util.App
{
    public class AppUtils
    {
        /// <summary>
        /// Get the root directory path of the executing assembly. Works even if it's called from another assembly
        /// </summary>
        /// <returns></returns>
        public static string AppDirectoryPath()
        {
            return new FileInfo(new Uri(Assembly.GetEntryAssembly().GetName().CodeBase).AbsolutePath).Directory.FullName;
        }

        public static string AppDirectoryPath(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
                throw new ArgumentNullException(nameof(file));

            return Path.Combine(new FileInfo(new Uri(Assembly.GetEntryAssembly().GetName().CodeBase).AbsolutePath).Directory.FullName, file);
        }

        public static string GetAssemblyName()
        {
            return Path.GetFileName(Assembly.GetEntryAssembly().GetName().CodeBase);
        }
    }
}
