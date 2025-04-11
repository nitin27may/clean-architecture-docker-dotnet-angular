using Contact.Application.Interfaces;
using Contact.Domain.Entities;
using Contact.Domain.Interfaces;
using Contact.Infrastructure.ExternalServices;
using Contact.Infrastructure.Persistence;
using Contact.Infrastructure.Persistence.Helper;
using Contact.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Contact.Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastrcutureServices(this IServiceCollection services, IConfiguration configuration)
    {
      

        services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
        services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
       

   

        services.AddScoped<IDapperHelper, DapperHelper>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IActivityLogRepository, ActivityLogRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
        services.AddScoped<IUserRoleRepository,UserRoleRepository>();
        services.AddScoped<IGenericRepository<ContactPerson>, ContactPersonRepository>();
        services.AddScoped<IContactPersonRepository, ContactPersonRepository>();
        services.AddScoped<IOperationRepository, OperationRepository>();


        services.AddScoped<IEmailService, EmailService>();
        return services;
    }
}
