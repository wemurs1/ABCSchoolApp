namespace ABCApp.Infrastructure.Services.Identity;

public interface IUserService
{
    Task<IResponseWrapper<string>> UpdateUserAsync(UpdateUserRequest request);
    Task<IResponseWrapper<string>> ChangeUserPassword(ChangePasswordRequest request);
    Task<IResponseWrapper<List<UserResponse>>> GetUsersAsync();
    Task<IResponseWrapper<UserResponse>> GetByIdAsync(string userId);
    Task<IResponseWrapper<string>> RegisterUserAsync(CreateUserRequest request);
    Task<IResponseWrapper<List<UserRoleResponse>>> GetUserRolesAsync(string userId);
    Task<IResponseWrapper<string>> UpdateUserRolesAsync(string userId, UserRolesRequest request);
}
