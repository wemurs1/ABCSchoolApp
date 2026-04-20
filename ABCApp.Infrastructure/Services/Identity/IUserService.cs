namespace ABCApp.Infrastructure.Services.Identity;

public interface IUserService
{
    Task<IResponseWrapper<string>> UpdateUserAsync(UpdateUserRequest request);
    Task<IResponseWrapper<string>> ChangeUserPassword(ChangePasswordRequest request);
}
