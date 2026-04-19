using System.Security.Claims;
using ABCShared.Library.Constants;

namespace ABCApp.Infrastructure.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GetEmail(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(ClaimTypes.Email)?.Value;
    }

    public static string? GetFirstName(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(ClaimTypes.Name)?.Value;
    }

    public static string? GetLastName(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(ClaimTypes.Surname)?.Value;
    }

    public static string? GetTenant(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(ClaimConstants.Tenant)?.Value;
    }

    public static string? GetUserId(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    public static string? GetPhoneNumber(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(ClaimTypes.MobilePhone)?.Value;
    }
}
