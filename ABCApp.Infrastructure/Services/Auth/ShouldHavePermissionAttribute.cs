using Microsoft.AspNetCore.Authorization;

namespace ABCApp.Infrastructure.Services.Auth;

public class ShouldHavePermissionAttribute : AuthorizeAttribute
{
    public ShouldHavePermissionAttribute(string action, string feature)
    {
        Policy = SchoolPermission.NameFor(action, feature);
    }
}
