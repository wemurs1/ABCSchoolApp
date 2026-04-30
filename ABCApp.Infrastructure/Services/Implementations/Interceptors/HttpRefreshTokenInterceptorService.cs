using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Toolbelt.Blazor;

namespace ABCApp.Infrastructure.Services.Implementations.Interceptors;

public class HttpRefreshTokenInterceptorService(
    HttpClientInterceptor httpClientInterceptor,
    ITokenService tokenService,
    NavigationManager navigation,
    ISnackbar snackbar
) : IHttpRefreshTokenInterceptorService
{
    private readonly HttpClientInterceptor _httpClientInterceptor = httpClientInterceptor;
    private readonly ITokenService _tokenService = tokenService;
    private readonly NavigationManager _navigation = navigation;
    private readonly ISnackbar _snackbar = snackbar;

    public async Task InterceptBeforeHttpRequestAsync(object sender, HttpClientInterceptorEventArgs eventArgs)
    {
        var absPath = eventArgs.Request.RequestUri?.AbsolutePath ?? "token";
        if (!absPath.Contains("token"))
        {
            try
            {
                var token = await _tokenService.ForceTryRefreshTokenAsync();
                if (!string.IsNullOrEmpty(token))
                {
                    eventArgs.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }
            catch (Exception)
            {
                _snackbar.Add("You are logged out", Severity.Error);
                await _tokenService.LogoutAsync();
                _navigation.NavigateTo("/");
            }
        }
    }

    public void RegisterEvent() => _httpClientInterceptor.BeforeSendAsync += InterceptBeforeHttpRequestAsync;
}
