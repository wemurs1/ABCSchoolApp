using ABCShared.Library.Models.Responses.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace ABCSchoolApp.Pages.Identity;

public partial class UserRoles
{
    [CascadingParameter] protected Task<AuthenticationState> AuthenticationState { get; set; } = default!;
    [Inject] protected IAuthorizationService AuthorizationService { get; set; } = default!;
    [Parameter] public string UserId { get; set; } = string.Empty;

    private List<UserRoleResponse> _userRoleList = [];
    private UserResponse _user = new()
    {
        Id = string.Empty,
        FirstName = string.Empty,
        LastName = string.Empty,
        Email = string.Empty,
        UserName = string.Empty,
        PhoneNumber = string.Empty
    };

    private bool _canUpdateUserRoles = false;
    private bool _isLoading = true;
    private string _title = string.Empty;
    private string _description = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthenticationState).User;
        _canUpdateUserRoles = await AuthorizationService.HasPermissionAsync(user, SchoolFeature.UserRoles, SchoolAction.Update);
        await GetUserByIdAsync();
        await GetUserRolesAsync();
        _isLoading = false;
    }

    private async Task GetUserByIdAsync()
    {
        var result = await _userService.GetByIdAsync(UserId);
        if (result.IsSuccessful)
        {
            _user = result.Data;
            _title = $"{_user.FirstName} {_user.LastName}";
            _description = $"Manager {_user.FirstName} {_user.LastName}'s roles";
        }
        else
        {
            foreach (var message in result.Messages)
            {
                _snackbar.Add(message, Severity.Error);
            }
        }
    }

    private async Task GetUserRolesAsync()
    {
        var result = await _userService.GetUserRolesAsync(UserId);
        if (result.IsSuccessful)
        {
            _userRoleList = result.Data;
        }
        else
        {
            foreach (var message in result.Messages)
            {
                _snackbar.Add(message, Severity.Error);
            }
        }
    }

    private async Task UpdateUserRolesAsync()
    {
        var request = new UserRolesRequest
        {
            UserRoles = _userRoleList.Select(x => new UserRoleRequest
            { RoleId = x.RoleId, Name = x.Name, Description = x.Description, IsAssigned = x.IsAssigned }
            ).ToList()
        };
        var result = await _userService.UpdateUserRolesAsync(UserId, request);
        if (result.IsSuccessful)
        {
            _snackbar.Add(result.Messages[0], Severity.Success);
            _navigation.NavigateTo("/users");
        }
        else
        {
            foreach (var message in result.Messages)
            {
                _snackbar.Add(message, Severity.Error);
            }
        }
    }

    private void Cancel()
    {
        _navigation.NavigateTo("/users");
    }
}
