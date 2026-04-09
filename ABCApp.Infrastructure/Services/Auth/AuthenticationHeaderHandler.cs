using System.Net.Http.Headers;
using ABCApp.Infrastructure.Constants;
using Blazored.LocalStorage;

namespace ABCApp.Infrastructure.Services.Auth;

public class AuthenticationHeaderHandler(ILocalStorageService localStorageService) : DelegatingHandler
{
    private readonly ILocalStorageService _localStorageService = localStorageService;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Headers.Authorization?.Scheme != "Bearer")
            {
                var savedToken = await _localStorageService.GetItemAsync<string>(StorageConstants.AuthToken, cancellationToken);
                if (!string.IsNullOrEmpty(savedToken))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);
                }
            }
            return await base.SendAsync(request, cancellationToken);
        }
        catch (System.Exception)
        {
            throw;
        }
    }
}
