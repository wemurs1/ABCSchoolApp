using System.Net.Http.Headers;
using ABCSchoolApp.Infrastructure.Constants;
using Blazored.LocalStorage;

namespace ABCSchoolApp.Infrastructure.Services.Auth;

public class AuthenticationHeaderHandler(ILocalStorageService storageService) : DelegatingHandler
{
    private readonly ILocalStorageService _storageService = storageService;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Headers.Authorization?.Scheme != "Bearer")
            {
                var savedToken = await _storageService.GetItemAsync<string>(StorageConstants.AuthToken, cancellationToken);
                if (!string.IsNullOrEmpty(savedToken))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);
                }
            }
            return await base.SendAsync(request, cancellationToken);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
