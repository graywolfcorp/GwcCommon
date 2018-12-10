using NLog;
using Gwc.Common.Interfaces.Services;

namespace Gwc.Common.Services
{
    public class LogManagerService : ILogManagerService
    {
        public string DefaultLogger { get =>  "NLogLogger"; }
        public string DefaultDbLogger { get => "CoreDbLogger";  }
        public string DefaultFileLogger { get => "CoreFileLogger";  }

        public ILoggingService GetLoggingService(string name)
        {
            var logger = LogManager.GetLogger(name);
            return new LoggingService(logger);
        }

        public ILoggingService GetDefaultDbLoggingService()
        {
            var logger = LogManager.GetLogger(DefaultDbLogger);
            return new LoggingService(logger);
        }

        public ILoggingService GetDefaultFileLoggingService()
        {
            var logger = LogManager.GetLogger(DefaultFileLogger);
            return new LoggingService(logger);
        }

       
    }
}
