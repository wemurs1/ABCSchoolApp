using ABCShared.Library.Models.Requests.Token;
using ABCShared.Library.Wrappers;

namespace ABCSchoolApp.Infrastructure.Services.Identity;

public interface ITokenService
{
    Task<IResponseWrapper> LoginAsync(string tenant, TokenRequest request);
}
