using Microsoft.AspNetCore.Authorization;

namespace Contact.Api.Core.Authorization;

public class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}
