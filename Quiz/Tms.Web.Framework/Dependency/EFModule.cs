using Autofac;
using Tms.DataAccess;
using Tms.DataAccess.Common;

namespace Tms.Web.Framework.Dependency
{
    public class EFModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new RepositoryModule());

            builder.RegisterType(typeof(QuizSystemEntities)).InstancePerRequest();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
        }
    }
}