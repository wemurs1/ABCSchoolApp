using System.Net.Http.Json;

namespace ABCApp.Infrastructure.Services.Implementations.Tenancy;

public class TenantService(HttpClient httpClient, ApiSettings apiSettings) : ITenantService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ApiSettings _apiSettings = apiSettings;

    public async Task<IResponseWrapper<string>> ActivateAsync(string tenantId)
    {
        var response = await _httpClient.PutAsJsonAsync(_apiSettings.TenantsEndpoints.FullActivate(tenantId), tenantId);
        var result = await response.WrapToResponse<string>() ?? throw new Exception("result is null");
        return result;
    }

    public async Task<IResponseWrapper<string>> CreateAsync(CreateTenantRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(_apiSettings.TenantsEndpoints.Create, request);
        var result = await response.WrapToResponse<string>() ?? throw new Exception("result is null");
        return result;
    }

    public async Task<IResponseWrapper<string>> DeActivateAsync(string tenantId)
    {
        var response = await _httpClient.PutAsJsonAsync(_apiSettings.TenantsEndpoints.FullDeactivate(tenantId), tenantId);
        var result = await response.WrapToResponse<string>() ?? throw new Exception("result is null");
        return result;
    }

    public async Task<IResponseWrapper<List<TenantResponse>>> GetAllAsync()
    {
        var response = await _httpClient.GetAsync(_apiSettings.TenantsEndpoints.All);
        var result = await response.WrapToResponse<List<TenantResponse>>() ?? throw new Exception("result is null");
        return result;
    }

    public async Task<IResponseWrapper<TenantResponse>> GetByIdAsync(string tenantId)
    {
        var response = await _httpClient.GetAsync(_apiSettings.TenantsEndpoints.GetById(tenantId));
        var result = await response.WrapToResponse<TenantResponse>() ?? throw new Exception("result is null");
        return result;
    }

    public async Task<IResponseWrapper<string>> UpgradeSubscriptionAsync(UpdateTenantSubscriptionRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync(_apiSettings.TenantsEndpoints.Upgrade, request);
        var result = await response.WrapToResponse<string>() ?? throw new Exception("result is null");
        return result;
    }
}
