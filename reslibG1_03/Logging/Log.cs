﻿using reslibG1_03.Util.App;
using System;
using System.Collections.Generic;
using System.IO;
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


        public static void SetConfig(string filePath) => SetConfig(filePath, null);

        public static void SetConfig(string filePath, LoggerOptions[] options)
        {
            if (_logger is not null)
                _logger.Dispose();

            _logger = new(filePath, options);
        }

        public static void SetConfig(string filePath, bool append)
        {
            if (_logger is not null)
                _logger.Dispose();

            _logger = new(filePath, append);
        }

        public static void StartLogging() => logger.LogStartup();
        public static void StartLogging(string message) => logger.LogStartup(message);

        public static void Info(string message) => logger.LogInfo(message);
        public static void Info(object obj) => logger.LogInfo(obj.ToString());

        public static void Message(string message) => logger.LogMessage(message);
        public static void Message(object obj) => logger.LogMessage(obj.ToString());

        public static void Error(string message) => logger.LogError(message);
        public static void Error(string message, Exception e) => logger.LogError(message, e);

        public static void Warning(string message) => logger.LogWarning(message);
        public static void Warning(string message, Exception e) => logger.LogWarning(message, e);

        public static void Fatal(string message) => logger.LogFatal(message);
        public static void Fatal(string message, Exception e) => logger.LogFatal(message, e);

        public static void Exception(string message, Exception e) => logger.LogException(message, e);

        public static void Values<T>(T obj) => logger.LogValues(obj);
        public static void Values<T>(T obj, string objName, string log = null) => logger.LogValues(obj, objName, log);
        public static void Values<T>(T obj, string objName, bool allowAllTypes, string log = null) => logger.LogValues(obj, objName, allowAllTypes, log);
        public static void Values<T>(T obj, string objName, Type[] allowedTypes, string log = null) => logger.LogValues(obj, objName, allowedTypes, log);
    }
}
