using Microsoft.AspNetCore.Authorization;

namespace CompanySys.API.Authorization;

public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permission)
        : base(policy: permission)
    {
    }
}