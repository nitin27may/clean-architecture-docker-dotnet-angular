using Contact.Application.Interfaces;
using Contact.Application.Mappings;
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
        // Register AutoMapper with all profiles in the assembly
        services.AddAutoMapper(cfg =>
        {
            // If you prefer, you can also add individual profiles like this:
            cfg.AddProfile<UserMappingProfile>();
        }, typeof(UserMappingProfile).Assembly);
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
