using System.Net.Http.Json;

namespace ABCApp.Infrastructure.Services.Implementations.Identity;

public class RoleService(HttpClient httpClient, ApiSettings apiSettings) : IRoleService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ApiSettings _apiSettings = apiSettings;

    public async Task<IResponseWrapper<string>> CreateAsync(CreateRoleRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(_apiSettings.RoleEndpoints.Create, request);
        var result = await response.WrapToResponse<string>() ?? throw new Exception("result is null");
        return result;
    }

    public async Task<IResponseWrapper<string>> DeleteAsync(string roleId)
    {
        var response = await _httpClient.DeleteAsync(_apiSettings.RoleEndpoints.DeleteRole(roleId));
        var result = await response.WrapToResponse<string>() ?? throw new Exception("result is null");
        return result;
    }

    public async Task<IResponseWrapper<List<RoleResponse>>> GetRolesAsync()
    {
        var response = await _httpClient.GetAsync(_apiSettings.RoleEndpoints.All);
        var result = await response.WrapToResponse<List<RoleResponse>>() ?? throw new Exception("result is null");
        return result;

    }

    public async Task<IResponseWrapper<RoleResponse>> GetRoleWithoutPermissionsAsync(string roleId)
    {
        var response = await _httpClient.GetAsync(_apiSettings.RoleEndpoints.GetByIdPartial(roleId));
        var result = await response.WrapToResponse<RoleResponse>() ?? throw new Exception("result is null");
        return result;
    }

    public async Task<IResponseWrapper<RoleResponse>> GetRoleWithPermissionsAsync(string roleId)
    {
        var response = await _httpClient.GetAsync(_apiSettings.RoleEndpoints.GetById(roleId));
        var result = await response.WrapToResponse<RoleResponse>() ?? throw new Exception("result is null");
        return result;
    }

    public async Task<IResponseWrapper<string>> UpdateAsync(UpdateRoleRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync(_apiSettings.RoleEndpoints.Update, request);
        var result = await response.WrapToResponse<string>() ?? throw new Exception("result is null");
        return result;
    }

    public async Task<IResponseWrapper<string>> UpdatePermissionsAsync(UpdateRolePermissionsRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync(_apiSettings.RoleEndpoints.UpdatePermission, request);
        var result = await response.WrapToResponse<string>() ?? throw new Exception("result is null");
        return result;
    }
}
