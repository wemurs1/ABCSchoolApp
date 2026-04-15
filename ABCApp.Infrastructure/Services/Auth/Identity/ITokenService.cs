namespace ABCApp.Infrastructure.Services.Auth.Identity;

public interface ITokenService
{
    Task<IResponseWrapper> LoginAsync(string tenant, TokenRequest request);
    Task<IResponseWrapper> LogoutAsync();
}
