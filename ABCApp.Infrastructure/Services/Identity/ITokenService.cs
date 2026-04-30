namespace ABCApp.Infrastructure.Services.Identity;

public interface ITokenService
{
    Task<IResponseWrapper> LoginAsync(string tenant, TokenRequest request);
    Task<IResponseWrapper> LogoutAsync();
    Task<string> RefreshTokenAsync();
    Task<string> ForceTryRefreshTokenAsync();
}
