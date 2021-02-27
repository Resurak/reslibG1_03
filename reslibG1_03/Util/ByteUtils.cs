using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reslibG1_03.Util
{
    public class ByteUtils
    {
        static string b = "B";
        static string kB = "kB";
        static string MB = "MB";
        static string GB = "GB";
        static string TB = "TB";

        public static string GetSizeString(long size)
        {
            if (size <= 0)
                return "0B";

            if (size > (int)SizeInBytes.B && size <= (int)SizeInBytes.KB)
                return $"{size}{b}";
            else if (size > (int)SizeInBytes.KB && size <= (int)SizeInBytes.MB)
                return $"{Math.Round((double)(size / (int)SizeInBytes.KB), 2)}{kB}";
            else if (size > (int)SizeInBytes.MB && size <= (int)SizeInBytes.GB)
                return $"{Math.Round((double)(size / (int)SizeInBytes.MB))}{MB}";
            else if (size > (int)SizeInBytes.GB && size <= (long)SizeInBytes.TB)
                return $"{Math.Round((double)(size / (int)SizeInBytes.GB))}{GB}";
            else
                return $"{Math.Round((double)(size / (long)SizeInBytes.TB))}{TB}";
        }
    }

    [Flags]
    public enum SizeInBytes : long
    {
        B = 1,
        KB = 1024,
        MB = 1_048_576,
        GB = 1_073_741_824,
        TB = 1_099_511_627_776,
    }
}
