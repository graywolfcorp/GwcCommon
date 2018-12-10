using Autofac;
using Autofac.Extensions.DependencyInjection;
using Gwc.Common.Interfaces.Services;
using Gwc.Common.Utilities.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.IO;

namespace Gwc.Testing.IntegrationTesting
{
    public class BaseIntegrationTestFixture
    {
        public static ILoggingService _loggingService;
        public static ILogManagerService _logManagerService;
        public static ISendGridService _sendGridService;
        public static string _sendGridApiKey;

        [SetUpFixture]
        public class IntegrationTests
        {            

            [OneTimeSetUp]
            public void Init()
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(TestContext.CurrentContext.TestDirectory)
                    .AddUserSecrets("8cef12cd-ce27-4c76-adca-413d9edc4ccd")
                    .AddEnvironmentVariables()
                    .Build();
                               
                var developmentMode = configuration["AppSettings:developmentMode"].ToLower() == "true";
                var logLevel = configuration["AppSettings:LogLevel"];
                var logFolder = configuration["AppSettings:LogFolder"];
                _sendGridApiKey = configuration["AppSettings:SendGridApiKey"];

                var services = new ServiceCollection();
                var _containerBuilder = new ContainerBuilder();
                AutofacConfig.Register(_containerBuilder, developmentMode);
                _containerBuilder.Populate(services);
                var container = _containerBuilder.Build();
                var serviceProvider = new AutofacServiceProvider(container);

                _logManagerService = serviceProvider.GetService<ILogManagerService>();
                _loggingService = serviceProvider.GetService<ILoggingService>();
                _sendGridService = serviceProvider.GetService<ISendGridService>();

                var connectionString = configuration["AppSettings:ConnectionString"];


                _loggingService.AddDatabaseTarget(connectionString, logLevel, _logManagerService.DefaultDbLogger);
                _loggingService.AddFileTarget(logFolder.Add("nLogTesting.txt"), logLevel, _logManagerService.DefaultFileLogger);
            }
        }
    }
}