using NUnit.Framework;
using Gwc.Common.Utilities.Extensions;
using System;
using System.Linq;

namespace Gwc.Testing.IntegrationTesting
{
    public class LoggingServiceIntegrationtestFixture : BaseIntegrationTestFixture
    {
        [Test]
        public void LoggingTest()
        {
            var logger = _logManagerService.GetDefaultDbLoggingService();
            logger.Info("created DbLogging Test 6");
            Assert.That(1 == 1);
        }

        [Test]
        public void LoggingTestDbByName()
        {
            var logger = _logManagerService.GetLoggingService(_logManagerService.DefaultDbLogger);
            logger.Info("created Logging Test by name");
            Assert.That(1 == 1);
        }

        [Test]
        public void LoggingTestDbException()
        {
            var zero = 0;
            var dbLogger = _logManagerService.GetDefaultDbLoggingService();
            var fileLogger = _logManagerService.GetDefaultFileLoggingService();

            try
            {
                var x = 1 / zero;
            }
            catch (Exception ex)
            {
                dbLogger.Error(ex, "test db message");
                fileLogger.Error(ex, "test file message");
            }
            Assert.That(1 == 1);
        }
    }
}