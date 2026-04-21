namespace ABCApp.Infrastructure.Services.Tenancy;

public interface ITenantService
{
    Task<IResponseWrapper<List<TenantResponse>>> GetAllAsync();
    Task<IResponseWrapper<TenantResponse>> GetByIdAsync(string tenantId);
    Task<IResponseWrapper<string>> CreateAsync(CreateTenantRequest request);
    Task<IResponseWrapper<string>> UpgradeSubscriptionAsync(UpdateTenantSubscriptionRequest request);
    Task<IResponseWrapper<string>> ActivateAsync(string tenantId);
    Task<IResponseWrapper<string>> DeActivateAsync(string tenantId);
}
