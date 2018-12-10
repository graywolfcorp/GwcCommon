namespace Gwc.Common.Interfaces.Services
{
    public interface ILogManagerService
    {
        string DefaultLogger { get; }
        string DefaultDbLogger { get; }
        string DefaultFileLogger { get; }
        ILoggingService GetLoggingService(string name);
        ILoggingService GetDefaultDbLoggingService();
        ILoggingService GetDefaultFileLoggingService();
    }
}
