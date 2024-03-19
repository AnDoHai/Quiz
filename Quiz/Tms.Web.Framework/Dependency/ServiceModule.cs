using System.Reflection;
using Autofac;
using Module = Autofac.Module;

namespace Tms.Web.Framework.Dependency
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("Tms.Services"))
                      .Where(t => t.Name.EndsWith("Service"))
                      .AsImplementedInterfaces()
                      .InstancePerLifetimeScope();
        }
    }
}