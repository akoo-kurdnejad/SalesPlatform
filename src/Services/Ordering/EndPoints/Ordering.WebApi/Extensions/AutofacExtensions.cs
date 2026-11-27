using Autofac;
using Ordering.ApplicationService.IoC;
using Ordering.DataAccess.IoC;

namespace Ordering.WebApi.Extensions
{
    public static class AutofacExtensions
    {
        public static void AddOrderingModules(this ContainerBuilder containerBuilder)
        {
            var applicationAssembly = typeof(OrderingApplicationModule).Assembly;
            containerBuilder.RegisterModule(new OrderingApplicationModule(applicationAssembly));
            containerBuilder.RegisterModule(new OrderingPersistenceModule());
        }
    }
}
