using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ABCApp.Infrastructure.Extensions;

public static class AuthorizationServiceExtensions
{
    public static async Task<bool> HasPermissionAsync(this IAuthorizationService service, ClaimsPrincipal user, string feature, string action)
    {
        return (await service.AuthorizeAsync(user, null, SchoolPermission.NameFor(action, feature))).Succeeded;
    }
}
