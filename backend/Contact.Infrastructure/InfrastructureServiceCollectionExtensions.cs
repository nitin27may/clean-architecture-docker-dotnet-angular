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

        services.AddScoped<IActivityLogRepository, ActivityLogRepository>();
        services.AddScoped<IGenericRepository<ContactPerson>, ContactPersonRepository>();
        services.AddScoped<IContactPersonRepository, ContactPersonRepository>();
        services.AddScoped<IGenericRepository<Operation>, OperationRepository>();
        services.AddScoped<IOperationRepository, OperationRepository>();
        services.AddScoped<IGenericRepository<Page>, PageRepository>();
        services.AddScoped<IPageRepository, PageRepository>();
        services.AddScoped<IGenericRepository<Permission>, PermissionRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IGenericRepository<RolePermission>, RolePermissionRepository>();
        services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
        services.AddScoped<IGenericRepository<Role>, RoleRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();     
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        
        services.AddScoped<IGenericRepository<PageOperationMapping>, PageOperationMappingRepository>();
        
        services.AddScoped<IEmailService, EmailService>();
        return services;
    }
}
