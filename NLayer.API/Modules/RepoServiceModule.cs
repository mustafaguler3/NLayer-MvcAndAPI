using Autofac;
using NLayer.Caching;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWorks;
using NLayer.Service.Mapping;
using NLayer.Service.Services;
using System.Reflection;
using Module = Autofac.Module;

namespace NLayer.API.Modules
{
    public class RepoServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>)).InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

            var apiAssembly = Assembly.GetExecutingAssembly();

            var repoAssembly = Assembly.GetAssembly(typeof(VtContext));//biz vtcontext verdik

            var serviceAssembly = Assembly.GetAssembly(typeof(MapProfile));//service katmanındaki bir tanesini ver biz mapprofile verdik

            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(i => i.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(i => i.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();

            //InstancePerLifetimeScope = Scope
            //InstancePerDependency = Transiet karşılık gelir

            builder.RegisterType<ProductServiceWithCaching>().As<IProductService>();

            base.Load(builder);
        }
    }
}
