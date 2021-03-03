using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reslibG1_03.Util.Extensions
{
    public static class ExtensionMethods
    {
        public static long[] GetParts(this long total, short partsNumber)
        {
            if (total <= 0)
                throw new NotSupportedException();

            if (partsNumber <= 0)
                throw new InvalidOperationException("Parts can't be lower than 1");

            var remainder = total % partsNumber;
            var parts = new long[partsNumber];

            for (int i = 0; i < parts.Length; i++) 
                if (i != parts.Length - 1)
                    parts[i] = (int)(total / parts.Length);
                else
                    parts[i] = (int)(total / parts.Length) + remainder;

            return parts;
        }
    }
}
