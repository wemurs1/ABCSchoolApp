using System.Net.Http.Json;
using ABCShared.Library.Models.Responses.Identity;

namespace ABCApp.Infrastructure.Services.Implementations.Identity;

public class UserService(HttpClient httpClient, ApiSettings apiSettings) : IUserService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ApiSettings _apiSettings = apiSettings;

    public async Task<IResponseWrapper<string>> ChangeUserPassword(ChangePasswordRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync(_apiSettings.UserEndpoints.ResetPassword, request);
        var result = await response.WrapToResponse<string>() ?? throw new Exception("result is null");
        return result;
    }

    public async Task<IResponseWrapper<UserResponse>> GetByIdAsync(string userId)
    {
        var response = await _httpClient.GetAsync(_apiSettings.UserEndpoints.GetById(userId));
        var result = await response.WrapToResponse<UserResponse>() ?? throw new Exception("result is null");
        return result;
    }

    public async Task<IResponseWrapper<List<UserResponse>>> GetUsersAsync()
    {
        var response = await _httpClient.GetAsync(_apiSettings.UserEndpoints.All);
        var result = await response.WrapToResponse<List<UserResponse>>() ?? throw new Exception("result is null");
        return result;
    }

    public async Task<IResponseWrapper<string>> UpdateUserAsync(UpdateUserRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync(_apiSettings.UserEndpoints.Update, request);
        var result = await response.WrapToResponse<string>() ?? throw new Exception("result is null");
        return result;
    }
}
