using NLog;
using System;

namespace Gwc.Common.Interfaces.Services
{
    public interface ILoggingService
    {
        void AddDatabaseTarget(string connectionString, string logLevel, string targetName);
        void AddFileTarget(string fileName, string logLevel, string targetName);
        void Debug(Exception exception);
        void Debug(string format, params object[] args);
        void Debug(Exception exception, string format, params object[] args);
        void Error(Exception exception);
        void Error(Exception exception, string format, params object[] args);
        void Fatal(Exception exception);
        void Fatal(Exception exception, string format, params object[] args);
        ILogger GetDefaultDbLogger();
        ILogger GetDefaultFileLogger();
        ILogger GetLoggingService<T>();
        ILogger GetLoggingService(string loggerName);
        ILogger GetLoggingService<T>(string loggerName);
        void Info(Exception exception);
        void Info(string format, params object[] args);
        void Info(Exception exception, string format, params object[] args);
        void Trace(Exception exception);
        void Trace(string format, params object[] args);
        void Trace(Exception exception, string format, params object[] args);
        void Warn(Exception exception);
        void Warn(string format, params object[] args);
        void Warn(Exception exception, string format, params object[] args);
    }
}