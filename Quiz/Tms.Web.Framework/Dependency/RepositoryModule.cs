using System.Reflection;
using Autofac;

namespace Tms.Web.Framework.Dependency
{
    public class RepositoryModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("Tms.DataAccess"))
                     .Where(t => t.Name.EndsWith("Repository"))
                     .AsImplementedInterfaces()
                     .InstancePerLifetimeScope();
        }
    }
}