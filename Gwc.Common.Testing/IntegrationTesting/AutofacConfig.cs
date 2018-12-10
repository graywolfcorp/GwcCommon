using Autofac;
using Gwc.Common.Interfaces.Services;
using Gwc.Common.Services;
using System.Reflection;

namespace Gwc.Testing.IntegrationTesting
{
    public static class AutofacConfig
    {
        private static bool _developmentMode;

        public static void Register(ContainerBuilder builder, bool developmentMode)
        {
            _developmentMode = developmentMode;
            builder.RegisterModule(new ServiceModule());
        }       

        public class ServiceModule : Autofac.Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                builder.RegisterAssemblyTypes(Assembly.Load("Gwc.Common.Services"))
                    .Where(t => t.Name.EndsWith("Service"))
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();

                builder.RegisterType<LoggingService>().As<ILoggingService>().InstancePerLifetimeScope();
                builder.RegisterType<LogManagerService>().As<ILogManagerService>().InstancePerLifetimeScope();
            }
        }
    }
}