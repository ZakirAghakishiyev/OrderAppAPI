using Autofac;
using OrderApp.Core.Services;
using OrderApp.Infrastructure.Auth;
using OrderApp.Infrastructure.Data;
using OrderApp.Infrastructure.Interceptors;
using OrderApp.SharedKernel.Interfaces;
using OrderApp.Web.Companies;
using OrderApp.Web.Orders.Create;
using OrderApp.Web.Roles;
using OrderApp.Web.Services;
using OrderApp.Web.Users;

namespace OrderApp.Web;

public class OrderAppModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<OrderEndpointService>()
            .As<IOrderEndpointService>()
            .InstancePerLifetimeScope();
        builder.RegisterType<CompanyEndpointService>()
            .As<ICompanyEndpointService>()
            .InstancePerLifetimeScope();
        builder.RegisterType<UserEndpointService>()
            .As<IUserEndpointService>()
            .InstancePerLifetimeScope();
        builder.RegisterType<RoleEndpointService>()
            .As<IRoleEndpointService>()
            .InstancePerLifetimeScope();
        builder.RegisterGeneric(typeof(EfRepository<>))
       .As(typeof(IRepository<>))
       .InstancePerLifetimeScope();

        builder.RegisterType<AuditSaveChangesInterceptor>()
            .AsSelf()
            .InstancePerLifetimeScope();

        builder.RegisterType<AuthenticatedUserAccessor>()
            .As<IAuthenticatedUserAccessor>()
            .InstancePerLifetimeScope();

        builder.RegisterType<HttpContextAccessor>()
               .As<IHttpContextAccessor>()
               .SingleInstance();

        // builder.RegisterType<HttpContextUserAccessor>()
        //        .As<IAuthenticatedUserAccessor>()
        //        .InstancePerLifetimeScope();

        builder.RegisterType<JwtTokenService>()
               .AsSelf()
               .InstancePerLifetimeScope();

    }

}
