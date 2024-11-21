using Microsoft.AspNetCore.Authorization;

namespace Contact.Api.Core.Attributes;

public class AuthorizePermissionAttribute : AuthorizeAttribute
{
    public AuthorizePermissionAttribute(string permission)
    {
        Policy = $"{permission}Policy";
    }
}
