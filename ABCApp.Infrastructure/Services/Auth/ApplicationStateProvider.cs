using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace ABCApp.Infrastructure.Services.Auth;

// public class ApplicationStateProvider(ILocalStorageService localStorageService, HttpClient httpClient) : AuthenticationStateProvider
// {
//     private readonly ILocalStorageService _localStorageService = localStorageService;
//     private readonly HttpClient _httpClient = httpClient;

//     // public override async Task<AuthenticationState> GetAuthenticationStateAsync()
//     // {
//     //     var savedToken = await _localStorageService.GetItemAsync<string>("jwt");
//     // }
// }
