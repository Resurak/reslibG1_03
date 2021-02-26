using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reslibG1_03.Logging
{
    public sealed class Log
    {
        private static volatile Logger _logger = null;
        private static readonly object _lock = new object();

        private static Logger logger
        {
            get
            {
                if (_logger is null)
                    lock (_lock)
                        if (_logger is null)
                            _logger = new();

                return _logger;
            }
        }



        public static void StartLogging() => logger.LogStartup();

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
