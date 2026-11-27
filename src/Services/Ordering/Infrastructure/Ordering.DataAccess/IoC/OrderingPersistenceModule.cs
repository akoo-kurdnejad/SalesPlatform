using Autofac;
using Ordering.ApplicationService.Common;
using Ordering.DataAccess.Base;
using Ordering.DataAccess.Ef.Repositories;
using Ordering.Domain.Models.Orders.Repository;

namespace Ordering.DataAccess.IoC
{
    public sealed class OrderingPersistenceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<OrderRepository>().As<IOrderRepository>().InstancePerLifetimeScope();
            builder.RegisterType<OrderRepository>().As<IOrderRepository>().InstancePerLifetimeScope();
        }
    }
}
