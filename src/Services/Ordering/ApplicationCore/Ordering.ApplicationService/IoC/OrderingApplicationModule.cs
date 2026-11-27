using Autofac;
using FluentValidation;
using Framework.Application.Commands;
using Framework.Application.Validation;
using System.Reflection;

namespace Ordering.ApplicationService.IoC
{
    public sealed class OrderingApplicationModule : Autofac.Module
    {
        private readonly Assembly _applicationAssembly;

        public OrderingApplicationModule(Assembly applicationAssembly)
        {
            _applicationAssembly = applicationAssembly;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(_applicationAssembly)
                .AsClosedTypesOf(typeof(IValidator<>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(_applicationAssembly)
                .AsClosedTypesOf(typeof(ICommandHandler<,>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterGenericDecorator(typeof(ValidationCommandHandlerDecorator<,>), typeof(ICommandHandler<,>));
        }
    }
}
