using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using ABCShared.Library.Constants;
using Microsoft.AspNetCore.Components.Authorization;

namespace ABCApp.Infrastructure.Services.Auth;

public class ApplicationStateProvider(ILocalStorageService localStorageService, HttpClient httpClient) : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorageService = localStorageService;
    private readonly HttpClient _httpClient = httpClient;
    public ClaimsPrincipal? AuthenticatedStateUser { get; set; }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var savedToken = await _localStorageService.GetItemAsync<string>(StorageConstants.AuthToken);
        if (string.IsNullOrEmpty(savedToken)) return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);
        var state = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(GetClaimsFromJwt(savedToken), "jwt")));
        AuthenticatedStateUser = state.User;
        return state;
    }

    public void MarkUserAsAuthenticated(string username)
    {
        var authenticatedUser = new ClaimsPrincipal(
            new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Email, username)
            ],
            "apiauth")
        );
        var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
        NotifyAuthenticationStateChanged(authState);
    }

    public void MarkUserAsLoggedOut()
    {
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        var authState = Task.FromResult(new AuthenticationState(anonymousUser));
        NotifyAuthenticationStateChanged(authState);
    }

    public async Task<ClaimsPrincipal> GetAuthenticationStateProviderUserAsync()
    {
        var state = await GetAuthenticationStateAsync();
        return state.User;
    }

    #region Helpers
    // private IEnumerable<Claim> GetClaimsFromJwt(string jwt)
    // {
    //     var handler = new JwtSecurityTokenHandler();
    //     var jwtToken = handler.ReadJwtToken(jwt);
    //     foreach (var claim in jwtToken.Claims)
    //     {
    //         Console.WriteLine($"Claim: {claim.Value}");
    //     }
    //     return jwtToken.Claims;        
    // }

    private IEnumerable<Claim> GetClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        if (keyValuePairs != null)
        {
            keyValuePairs.TryGetValue(ClaimTypes.Role, out var roles);

            if (roles != null)
            {
                if (roles.ToString()!.Trim().StartsWith('[')) // we have an array
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString()!);
                    claims.AddRange(parsedRoles!.Select(roleName => new Claim(ClaimTypes.Role, roleName)));
                }
                else // we have a single value
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()!));
                }
                keyValuePairs.Remove(ClaimTypes.Role);
            }

            keyValuePairs.TryGetValue(ClaimConstants.Permission, out var permissions);

            if (permissions != null)
            {
                if (permissions.ToString()!.Trim().StartsWith('['))
                {
                    var parssedPermissions = JsonSerializer.Deserialize<string[]>(permissions.ToString()!);
                    claims.AddRange(parssedPermissions!.Select(permissionName => new Claim(ClaimConstants.Permission, permissionName)));
                }
                else
                {
                    claims.Add(new Claim(ClaimConstants.Permission, permissions.ToString()!));
                }
                keyValuePairs.Remove(ClaimConstants.Permission);
            }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!)));
        }
        return claims;
    }

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
