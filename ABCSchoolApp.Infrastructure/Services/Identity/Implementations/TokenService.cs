using System.Net.Http.Headers;
using System.Net.Http.Json;
using ABCSchoolApp.Infrastructure.Constants;
using ABCSchoolApp.Infrastructure.Extensions;
using ABCSchoolApp.Infrastructure.Services.Auth;
using ABCShared.Library.Models.Requests.Token;
using ABCShared.Library.Models.Responses.Token;
using ABCShared.Library.Wrappers;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace ABCSchoolApp.Infrastructure.Services.Identity.Implementations;

public class TokenService(
    HttpClient httpClient,
    ILocalStorageService localStorageService,
    AuthenticationStateProvider authenticationStateProvider,
    ApiSettings apiSettings) : ITokenService
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
        AddTenantHeader(httpRequest, headerName: "tenant", value: tenant);

        var response = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        var result = await response.WrapToResponse<TokenResponse>();

        if (result.IsSuccessful)
        {
            var token = result.Data.Jwt;
            var refreshToken = result.Data.RefreshToken;
            await _localStorageService.SetItemAsync(StorageConstants.AuthToken, token);
            await _localStorageService.SetItemAsync(StorageConstants.RefreshToken, refreshToken);

            ((ApplicationStateProvider)_authenticationStateProvider).MarkUserAuthenticated(request.Username);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return ResponseWrapper.Success();
        }
        else
        {
            return ResponseWrapper.Fail(result.Messages);
        }
    }

    #region Helpers
    private static void AddTenantHeader(HttpRequestMessage request, string headerName, string value)
    {
        if (string.IsNullOrEmpty(value) || request.Headers.Contains(headerName)) return;

        request.Headers.TryAddWithoutValidation(headerName, value);
    }
    #endregion Helpers
}
