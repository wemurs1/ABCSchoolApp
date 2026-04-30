using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ABCApp.Infrastructure.Services.Implementations.Identity;

public class TokenService(
    HttpClient httpClient,
    ILocalStorageService localStorageService,
    AuthenticationStateProvider authenticationStateProvider,
    ApiSettings apiSettings
) : ITokenService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILocalStorageService _localStorageService = localStorageService;
    private readonly AuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider;
    private readonly ApiSettings _apiSettings = apiSettings;

    public async Task<IResponseWrapper> LoginAsync(string tenant, TokenRequest request)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, _apiSettings.TokenEndpoints.Login)
        {
            Content = JsonContent.Create(request)
        };
        AddTenantHeader(httpRequest, headerName: "tenant", headerValue: tenant);

        var response = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        var result = await response.WrapToResponse<TokenResponse>() ?? throw new Exception("No result from login request");
        if (result.IsSuccessful)
        {
            await UpdateUserState(result.Data);
            return ResponseWrapper.Success();
        }
        else
        {
            return ResponseWrapper.Fail(result.Messages);
        }
    }

    public async Task<IResponseWrapper> LogoutAsync()
    {
        await _localStorageService.RemoveItemAsync(StorageConstants.AuthToken);
        await _localStorageService.RemoveItemAsync(StorageConstants.RefreshToken);
        ((ApplicationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
        _httpClient.DefaultRequestHeaders.Authorization = null;
        return ResponseWrapper.Success();
    }

    public async Task<string> RefreshTokenAsync()
    {
        var currentJwt = await _localStorageService.GetItemAsync<string>(StorageConstants.AuthToken);
        var currentRefreshToken = await _localStorageService.GetItemAsync<string>(StorageConstants.RefreshToken);
        var request = new RefreshTokenRequest
        {
            CurrentJwt = currentJwt,
            CurrentRefreshToken = currentRefreshToken
        };
        var response = await _httpClient.PostAsJsonAsync(_apiSettings.TokenEndpoints.RefreshToken, request);
        var result = await response.WrapToResponse<TokenResponse>() ?? throw new Exception("Result is null");
        if (result.IsSuccessful)
        {
            await UpdateUserState(result.Data);
            return currentJwt;
        }
        else
        {
            throw new ApplicationException(result.Messages[0]);
        }
    }

    public async Task<string> ForceTryRefreshTokenAsync()
    {
        var currentRefreshToken = await _localStorageService.GetItemAsync<string>(StorageConstants.RefreshToken);
        if (string.IsNullOrEmpty(currentRefreshToken)) return string.Empty;

        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();

        var user = authState.User;
        var exp = user.FindFirst(c => c.Type.Equals("exp"))?.Value;
        var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
        var currentTime = DateTime.UtcNow;

        var diff = expTime - currentTime;

        // Only within last five minutes to expiry
        if (diff.TotalMinutes <= 5)
        {
            if (diff.TotalMinutes < 0)
            {
                await LogoutAsync();
            }
            else
            {
                return await RefreshTokenAsync();
            }
        }
        return string.Empty;
    }

    private async Task UpdateUserState(TokenResponse response)
    {
        var token = response.Jwt;
        var refreshToken = response.RefreshToken;
        await _localStorageService.SetItemAsync(StorageConstants.AuthToken, token);
        await _localStorageService.SetItemAsync(StorageConstants.RefreshToken, refreshToken);
        ((ApplicationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    private static void AddTenantHeader(HttpRequestMessage request, string headerName, string headerValue)
    {
        if (string.IsNullOrEmpty(headerValue) || request.Headers.Contains(headerName)) return;

        request.Headers.TryAddWithoutValidation(headerName, headerValue);
    }
}
