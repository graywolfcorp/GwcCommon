using NLog;
using NLog.Config;
using NLog.Targets;
using Gwc.Common.Interfaces.Services;
using System;

namespace Gwc.Common.Services
{
    public class LoggingService : ILoggingService
    {
        private readonly ILogger _log;

        public LoggingService(ILogger log)
        {
            _log = log;
        }
        public LoggingService()
        {
            if (LogManager.Configuration == null)
            {
                LogManager.Configuration = new LoggingConfiguration();
            }
        }

        public void AddDatabaseTarget(string connectionString, string logLevel, string targetName)
        {
            LogLevel loggingLevel = LogLevel.Off;
            DatabaseTarget target = new DatabaseTarget(targetName);
            DatabaseParameterInfo param;

            target.DBProvider = "mssql";
            target.ConnectionString = connectionString;
            target.CommandText = "insert into GwcLogs(LogDate,Level,Logger,Method,Message,Exception) values(@time_stamp, @level, @logger, @method,@message,@exception);";

            param = new DatabaseParameterInfo();
            param.Name = "@time_stamp";
            param.Layout = "${date}";
            target.Parameters.Add(param);

            param = new DatabaseParameterInfo();
            param.Name = "@level";
            param.Layout = "${level}";
            target.Parameters.Add(param);

            param = new DatabaseParameterInfo();
            param.Name = "@logger";
            param.Layout = "${callsite:className=true:includeSourcePath=false:methodName=false}";
            target.Parameters.Add(param);

            param = new DatabaseParameterInfo();
            param.Name = "@method";
            param.Layout = "${callsite:className=false:includeSourcePath=false:methodName=true}";
            target.Parameters.Add(param);

            param = new DatabaseParameterInfo();
            param.Name = "@message";
            param.Layout = "${message}";
            target.Parameters.Add(param);

            param = new DatabaseParameterInfo();
            param.Name = "@exception";
            param.Layout = "${exception:format=Message,StackTrace,Data}";
            target.Parameters.Add(param);

            ConfigureLogger(targetName, target, logLevel);
        }
        public void AddFileTarget(string fileName, string logLevel, string targetName)
        {
            LogLevel loggingLevel = LogLevel.Off;
            FileTarget target = new FileTarget(targetName) { FileName = fileName };

            target.Layout = "${longdate}|${uppercase:${level}}|${callsite:className=true:includeSourcePath=false:methodName=false}|${callsite:className=false:includeSourcePath=false:methodName=true} |${message} ${exception:format=Message,StackTrace,Data}";
            ConfigureLogger(targetName, target, logLevel);
        }
        public void Debug(Exception exception, string format, params object[] args)
        {
            if (!_log.IsDebugEnabled) return;
            var logEvent = GetLogEvent(_log.Name, LogLevel.Debug, exception, format, args);
            _log.Log(typeof(LoggingService), logEvent);
        }
        public void Debug(string format, params object[] args)
        {
            if (!_log.IsDebugEnabled) return;
            var logEvent = GetLogEvent(_log.Name, LogLevel.Debug, null, format, args);
            _log.Log(typeof(LoggingService), logEvent);
        }
        public void Debug(Exception exception)
        {
            this.Debug(exception, string.Empty);
        }
        public void Error(Exception exception, string format, params object[] args)
        {
            if (!_log.IsErrorEnabled) return;
            var logEvent = GetLogEvent(_log.Name, LogLevel.Error, exception, format, args);
            _log.Log(typeof(LoggingService), logEvent);
        }
        public void Error(Exception exception)
        {
            this.Error(exception, string.Empty);
        }
        public void Fatal(Exception exception, string format, params object[] args)
        {
            if (!_log.IsFatalEnabled) return;
            var logEvent = GetLogEvent(_log.Name, LogLevel.Fatal, exception, format, args);
            _log.Log(typeof(LoggingService), logEvent);
        }
        public void Fatal(Exception exception)
        {
            this.Fatal(exception, string.Empty);
        }
        public ILogger GetDefaultDbLogger()
        {
            ILogger logger = LogManager.GetLogger("CoreDbLogger");
            return logger;
        }
        public ILogger GetDefaultFileLogger()
        {
            ILogger logger = LogManager.GetLogger("CoreFileLogger");
            return logger;
        }
        public ILogger GetLoggingService<T>()
        {
            ILogger logger = LogManager.GetLogger("NLogLogger", typeof(T));
            return logger;
        }
        public ILogger GetLoggingService<T>(string loggerName)
        {
            ILogger logger = LogManager.GetLogger(loggerName, typeof(T));
            return logger;
        }
        public ILogger GetLoggingService(string loggerName)
        {
            ILogger logger = LogManager.GetLogger(loggerName);
            return logger;
        }
        public void Info(string format, params object[] args)
        {
            if (!_log.IsInfoEnabled) return;
            var logEvent = GetLogEvent(_log.Name, LogLevel.Info, null, format, args);
            _log.Log(typeof(LoggingService), logEvent);
        }
        public void Info(Exception exception, string format, params object[] args)
        {
            if (!_log.IsInfoEnabled) return;
            var logEvent = GetLogEvent(_log.Name, LogLevel.Info, exception, format, args);
            _log.Log(typeof(LoggingService), logEvent);
        }
        public void Info(Exception exception)
        {
            this.Info(exception, string.Empty);
        }
        public void Trace(Exception exception, string format, params object[] args)
        {
            if (!_log.IsTraceEnabled) return;
            var logEvent = GetLogEvent(_log.Name, LogLevel.Trace, exception, format, args);
            _log.Log(typeof(LoggingService), logEvent);
        }
        public void Trace(string format, params object[] args)
        {
            if (!_log.IsTraceEnabled) return;
            var logEvent = GetLogEvent(_log.Name, LogLevel.Trace, null, format, args);
            _log.Log(typeof(LoggingService), logEvent);
        }
        public void Trace(Exception exception)
        {
            this.Trace(exception, string.Empty);
        }
        public void Warn(Exception exception, string format, params object[] args)
        {
            if (!_log.IsWarnEnabled) return;
            var logEvent = GetLogEvent(_log.Name, LogLevel.Warn, exception, format, args);
            _log.Log(typeof(LoggingService), logEvent);
        }
        public void Warn(string format, params object[] args)
        {
            if (!_log.IsWarnEnabled) return;
            var logEvent = GetLogEvent(_log.Name, LogLevel.Warn, null, format, args);
            _log.Log(typeof(LoggingService), logEvent);
        }
        public void Warn(Exception exception)
        {
            this.Warn(exception, string.Empty);
        }
        private void ConfigureLogger(string loggerNamePattern, Target target, string logLevel)
        {
            var loggingLevel = LogLevel.Trace;

            switch (logLevel.ToUpper())
            {
                case "DEBUG":
                    loggingLevel = LogLevel.Debug;
                    break;

                case "INFO":
                    loggingLevel = LogLevel.Info;
                    break;

                case "WARN":
                    loggingLevel = LogLevel.Warn;
                    break;

                case "FATAL":
                    loggingLevel = LogLevel.Fatal;
                    break;

                case "OFF":
                    loggingLevel = LogLevel.Off;
                    break;
            }

            LogManager.Configuration.AddTarget(target);
            LoggingRule rule = new LoggingRule(loggerNamePattern, loggingLevel, target);
            LogManager.Configuration.LoggingRules.Add(rule);
            LogManager.ReconfigExistingLoggers();
        }
        private LogEventInfo GetLogEvent(string loggerName, LogLevel level, Exception exception, string format, object[] args)
        {
            var logEvent = new LogEventInfo (level, loggerName, string.Format(format, args));

            if (exception != null)
            {
                logEvent.Exception = exception;
            }
            return logEvent;
        }
    }
}