using reslibG1_03.Util.App;
using reslibG1_03.Util.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reslibG1_03.Logging
{
    public sealed class Logger
    {
        private const string defaultFileName = "outputLog.txt";
        private readonly object _lock = new object();


        private StreamWriter LogWriter = null;
        private FileStream LogStream = null;


        private string fileOutput = null;
        private LoggerOptions[] Options = null;

        private bool noTime => Options is null ? false : Options.Length > 0 && Options.Contains(LoggerOptions.NoTime) ? true : false;
        private bool noInfo => Options is null ? false : Options.Length > 0 && Options.Contains(LoggerOptions.NoInfo) ? true : false;


        public string FileOutput
        {
            get
            {
                return string.IsNullOrWhiteSpace(fileOutput) ? Path.Combine(AppUtils.AppDirectoryPath(), defaultFileName) : fileOutput;
            }
            set
            {
                fileOutput = value;
            }
        }


        public Logger() : this(null, null) { }

        public Logger(string output) : this(output, null) { }

        public Logger(LoggerOptions[] options) : this(null, options) { }

        public Logger(string output, LoggerOptions[] options) => Initialize(output, options);



        internal void Initialize(string output, LoggerOptions[] options)
        {
            FileOutput = output;
            Options = options;

            LogStream = new(FileOutput, FileMode.Create, FileAccess.Write, FileShare.Write);
            LogWriter = new(LogStream, Encoding.Unicode, 1024 * 64);
            LogWriter.AutoFlush = true;
        }


        internal void LogStartup() => LogStartup(null);

        internal void LogStartup(string message)
        {
            var log = "";

            if (message is null)
            {
                var sb = new StringBuilder();
                sb.Append('-', 100).AppendLine();
                sb.Append('-', 25).Append(" || Initializing Logger from " + AppUtils.GetAssemblyName()).AppendLine();
                sb.Append('-', 25).Append(" || Start time: " + DateTime.Now.ToString()).AppendLine();
                sb.Append('-', 100).AppendLine();

                log = sb.ToString();
            }
            else log = message;

            LogWriter.WriteLine(log);
        }

        internal void LogInfo(string log) => ProcessLog(log);

        internal void LogMessage(string log) => ProcessLog(log, LogType.MESSAGE);

        internal void LogError(string log) => ProcessLog(log, LogType.ERROR);
        internal void LogError(string log, Exception e) => ProcessLog(log, LogType.ERROR, e);

        internal void LogWarning(string log) => ProcessLog(log, LogType.WARNING);
        internal void LogWarning(string log, Exception e) => ProcessLog(log, LogType.WARNING, e);

        internal void LogFatal(string log) => ProcessLog(log, LogType.FATAL);
        internal void LogFatal(string log, Exception e) => ProcessLog(log, LogType.FATAL, e);

        internal void LogException(string log, Exception e) => ProcessLog(log, LogType.EXCEPTION, e);

        internal void LogValues<T>(T obj) => LogValues(obj, null, false, null);
        internal void LogValues<T>(T obj, string objName) => LogValues(obj, objName, false, null);
        internal void LogValues<T>(T obj, string objName, string log) => LogValues(obj, objName, false, log);
        internal void LogValues<T>(T obj, string objName, bool allowAllTypes, string log) => ProcessLog(obj, objName, allowAllTypes, null, log);
        internal void LogValues<T>(T obj, string objName, Type[] allowedTypes) => LogValues(obj, objName, allowedTypes, null);
        internal void LogValues<T>(T obj, string objName, Type[] allowedTypes, string log) => ProcessLog(obj, objName, false, allowedTypes, log);



        internal void ProcessLog(string log, LogType type = LogType.INFO, Exception e = null)
        {
            lock (_lock)
            {
                var sb = new StringBuilder();
                var time = DateTime.Now.ToString();
                var divisor = " || ";

                sb.Append($"{(noTime ? "" : (time + divisor))}{(noInfo ? "" : ($"[{type.ToString()}]" + divisor))}");
                sb.Append(log ?? "_NO-LOG_");

                if (e is not null)
                {
                    sb.Append("\n").Append('-', 25);
                    sb.Append("\n").Append("Exception message: ").Append(e.Message);
                    sb.Append("\n").Append("Exception StackTrace: ").Append("\n").Append(e.StackTrace);
                    sb.Append("\n").Append('-', 25);
                }

                SaveLog(sb);
            }
        }


        internal void ProcessLog<T>(T obj, string objName, bool allowAll, Type[] types, string log, LogType type = LogType.INFO, Exception e = null)
        {
            lock (_lock)
            {
                var sb = new StringBuilder();
                var time = DateTime.Now.ToString();
                var divisor = " || ";

                sb.Append($"{(noTime ? "" : (time + divisor))}{(noInfo ? "" : ($"[{type.ToString()}]" + divisor))}");
                sb.Append(log ?? $"Logging values of {objName ?? "1_no-name-provided_"}").AppendLine();
                sb.Append('-', 100).AppendLine();

                var valueList = ReflectionUtils.GetValuesFromObj(obj);

                foreach (var prop in valueList)
                {
                    sb.Append('-', prop.NestedIndex + 1).Append(divisor)
                        .Append("Type: ").Append(prop.Type).Append(divisor)
                        .Append("Name: ").Append(prop.Name).Append(divisor)
                        .Append("Value: ").Append(prop.Value)
                        .AppendLine();
                }

                SaveLog(sb);
            }
        }



        internal void SaveLog(string log) => LogWriter.WriteLine(log);
        internal void SaveLog(object log) => LogWriter.WriteLine(log);
    }


    public enum LoggerOptions
    {
        NoTime,
        NoInfo
    }


    public enum LogType
    {
        INFO,
        MESSAGE,
        ERROR,
        WARNING,
        FATAL,
        EXCEPTION
    }
}
