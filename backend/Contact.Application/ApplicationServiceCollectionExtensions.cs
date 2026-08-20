using Contact.Application.Interfaces;
using Contact.Application.Mapping;
using Contact.Application.Services;
using Contact.Application.UseCases.Users;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Contact.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        // Mapperly-backed replacement for AutoMapper — see
        // docs/adr/0001-permissive-license-dependency-policy.md for why, and
        // Mapping/ObjectMapper.cs for how the (source, destination) dispatch works.
        services.AddScoped<IMapper, ObjectMapper>();
        services.AddValidatorsFromAssemblyContaining<RegisterUserValidator>();
        services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

        services.AddScoped<IActivityLogService,ActivityLogService>();
        services.AddScoped<IContactPersonService,ContactPersonService>();
        services.AddScoped<IOperationService, OperationService>();
        services.AddScoped<IPageService, PageService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IRolePermissionService, RolePermissionService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserService, UserService>();    
        
        return services;
    }
}
