using Contact.Api.Core.Authorization;
using Contact.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Contact.Api.Core.Middleware;

public class CustomAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options, IServiceProvider serviceProvider) : IAuthorizationPolicyProvider
{
    private readonly DefaultAuthorizationPolicyProvider _fallbackPolicyProvider = new(options);

    public async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    {
        // Use a scope to resolve scoped services
        using var scope = serviceProvider.CreateScope();
        var permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();
        var permissionMappings = await permissionService.GetAllPageOperationMappingsAsync();

        if (permissionMappings.Any(mapping => $"{mapping.PageName}.{mapping.OperationName}Policy" == policyName))
        {
            var policy = new AuthorizationPolicyBuilder();
            policy.AddRequirements(new PermissionRequirement(policyName));
            return policy.Build();
        }

        // Fallback to the default provider for other policies
        return await _fallbackPolicyProvider.GetPolicyAsync(policyName);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => 
        _fallbackPolicyProvider.GetDefaultPolicyAsync();

    public Task<AuthorizationPolicy> GetFallbackPolicyAsync() => 
        _fallbackPolicyProvider.GetFallbackPolicyAsync();
}
