using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reslibG1_03.Logging
{
    public class Log
    {
        internal static Logger logger = new();

        public static void Info(string message) => logger.LogInfo(message);

        public static void Message(string message) => logger.LogMessage(message);

        public static void Error(string message) => logger.LogError(message);
        public static void Error(string message, Exception e) => logger.LogError(message, e);

        public static void Warning(string message) => logger.LogWarning(message);
        public static void Warning(string message, Exception e) => logger.LogWarning(message, e);

        public static void Fatal(string message) => logger.LogFatal(message);
        public static void Fatal(string message, Exception e) => logger.LogFatal(message, e);

        public static void Exception(string message, Exception e) => logger.LogException(message, e);
    }
}
