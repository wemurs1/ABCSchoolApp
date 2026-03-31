using ABCSchoolApp.Infrastructure.Constants;
// using ABCShared.Library.Constants;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
// using System.Text.Json;

namespace ABCSchoolApp.Infrastructure.Services.Auth;

public class ApplicationStateProvider(ILocalStorageService localStorageService, HttpClient httpClient) : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorageService = localStorageService;
    private readonly HttpClient _httpClient = httpClient;
    public ClaimsPrincipal? AuthenticatedStateUser { get; set; }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var savedToken = await _localStorageService.GetItemAsync<string>(StorageConstants.AuthToken);
        if (string.IsNullOrEmpty(savedToken))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

        var state = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(GetClaimsFromJwt(savedToken), "jwt")));
        AuthenticatedStateUser = state.User;
        return state;
    }

    public void MarkUserAuthenticated(string username)
    {
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Email, username)], "apiauth"));
        var authState = Task.FromResult(new AuthenticationState(anonymousUser));

        NotifyAuthenticationStateChanged(authState);
    }

    public void MarkUserAsLoggedOut()
    {
        var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity());
        var authState = Task.FromResult(new AuthenticationState(authenticatedUser));

        NotifyAuthenticationStateChanged(authState);
    }

    public async Task<ClaimsPrincipal> GetAuthenticationStateUserAsync()
    {
        var state = await GetAuthenticationStateAsync();
        return state.User;
    }

    #region Helpers
    private IEnumerable<Claim> GetClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        try
        {
            var handler = new JwtSecurityTokenHandler();

            // Validate format
            if (!handler.CanReadToken(jwt))
            {
                throw new Exception("Invalid JWT format.");
            }

            // Decode without validating signature (for reading only)
            var token = handler.ReadJwtToken(jwt);

            Console.WriteLine("=== Claims in JWT ===");
            foreach (Claim claim in token.Claims)
            {
                claims.Add(new Claim(claim.Type, claim.Value));
            }

            return claims;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    // private IEnumerable<Claim> GetClaimsFromJwt(string jwt)
    // {
    //     var claims = new List<Claim>();
    //     var payload = jwt.Split('.')[1];
    //     var jsonBytes = ParseBase64WithoutPadding(payload);

    //     var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

    //     if (keyValuePairs != null)
    //     {
    //         // keyValuePairs.TryGetValue(ClaimTypes.Role, out var roles);

    //         // if (roles != null)
    //         // {
    //         //     if (roles.ToString()!.Trim().StartsWith('['))
    //         //     {
    //         //         var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString()!);
    //         //         claims.AddRange(parsedRoles!.Select(roleName => new Claim(ClaimTypes.Role, roleName)));
    //         //     }
    //         //     else
    //         //     {
    //         //         claims.Add(new Claim(ClaimTypes.Role, roles.ToString()!));
    //         //     }

    //         //     keyValuePairs.Remove(ClaimTypes.Role);
    //         // }
    //         ProcessArrayClaim(ClaimTypes.Role, keyValuePairs, claims);

    //         // keyValuePairs.TryGetValue(ClaimConstants.Permission, out var permissions);

    //         // if (permissions != null)
    //         // {
    //         //     if (permissions.ToString()!.Trim().StartsWith('['))
    //         //     {
    //         //         var parsedPermissions = JsonSerializer.Deserialize<string[]>(permissions.ToString()!);
    //         //         claims.AddRange(parsedPermissions!.Select(permissionName => new Claim(ClaimConstants.Permission, permissionName)));
    //         //     }
    //         //     else
    //         //     {
    //         //         claims.Add(new Claim(ClaimConstants.Permission, permissions.ToString()!));
    //         //     }

    //         //     keyValuePairs.Remove(ClaimConstants.Permission);
    //         // }
    //         ProcessArrayClaim(ClaimConstants.Permission, keyValuePairs, claims);

    //         claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!)));
    //     }

    //     return claims;
    // }

    // private void ProcessArrayClaim(string claimName, Dictionary<string, object> claimDictionary, List<Claim> claims)
    // {
    //     claimDictionary.TryGetValue(claimName, out var output);

    //     if (output != null)
    //     {
    //         if (output.ToString()!.Trim().StartsWith('['))
    //         {
    //             var parsedRoles = JsonSerializer.Deserialize<string[]>(output.ToString()!);
    //             claims.AddRange(parsedRoles!.Select(name => new Claim(claimName, name)));
    //         }
    //         else
    //         {
    //             claims.Add(new Claim(claimName, output.ToString()!));
    //         }

    //         claimDictionary.Remove(ClaimTypes.Role);
    //     }
    // }

    private byte[] ParseBase64WithoutPadding(string base64Payload)
    {
        switch (base64Payload.Length % 4)
        {
            case 2: base64Payload += "=="; break;
            case 3: base64Payload += "="; break;
        }

        return Convert.FromBase64String(base64Payload);
    }
    #endregion Helpers
}
