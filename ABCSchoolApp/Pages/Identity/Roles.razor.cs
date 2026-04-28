using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace ABCSchoolApp.Pages.Identity;

public partial class Roles
{
    [CascadingParameter] protected Task<AuthenticationState> AuthenticationState { get; set; } = default!;
    [Inject] protected IAuthorizationService AuthorizationService { get; set; } = default!;

    private List<RoleResponse> _roleList = [];

    private bool _isLoading = true;
    private bool _canCreateroles = false;
    private bool _canUpdateRoles = false;
    private bool _canDeleteRoles = false;
    private bool _canViewRolePermissions = false;

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthenticationState).User;

        _canCreateroles = await AuthorizationService.HasPermissionAsync(user, SchoolFeature.Roles, SchoolAction.Create);
        _canUpdateRoles = await AuthorizationService.HasPermissionAsync(user, SchoolFeature.Roles, SchoolAction.Update);
        _canDeleteRoles = await AuthorizationService.HasPermissionAsync(user, SchoolFeature.Roles, SchoolAction.Delete);
        _canViewRolePermissions = await AuthorizationService.HasPermissionAsync(user, SchoolFeature.RoleClaims, SchoolAction.Read);

        await LoadRolesAsync();
        _isLoading = false;
    }

    private async Task LoadRolesAsync()
    {
        var result = await _roleService.GetRolesAsync();
        if (result.IsSuccessful)
        {
            _roleList = result.Data;
        }
        else
        {
            foreach (var message in result.Messages)
            {
                _snackbar.Add(message, Severity.Error);
            }
        }
    }

    private async Task InvokeCreateRoleDialog()
    {
        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Small,
            BackdropClick = false,
            FullWidth = true
        };

        var dialog = await _dialogService.ShowAsync<CreateRole>(title: null, options: options);
        var result = (await dialog.Result)!;
        if (!result.Canceled)
        {
            await LoadRolesAsync();
        }
    }

    private void InvokeUpdateRole(RoleResponse response)
    {
        _navigation.NavigateTo($"/update-role/{response.Id}");
    }

    private async Task DeleteRoleAsync(RoleResponse role)
    {
        var parameters = new DialogParameters
        {
            { nameof(Confirmation.Title), "Delete Role" },
            { nameof(Confirmation.Message), $"Are you sure you want to delete the role: {role.Name}?" },
            { nameof(Confirmation.ButtonText), "Delete" },
            { nameof(Confirmation.Color), Color.Error },
            { nameof(Confirmation.InputIcon), Icons.Material.Filled.CloudOff }
        };

        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Small,
            BackdropClick = true,
            FullWidth = true
        };

        var dialog = await _dialogService.ShowAsync<Confirmation>(title: null, parameters, options);
        var result = await dialog.Result ?? throw new Exception("result is null");
        if (!result.Canceled)
        {
            var response = await _roleService.DeleteAsync(role.Id);

            if (response.IsSuccessful)
            {
                _snackbar.Add(response.Messages[0], Severity.Success);

                await LoadRolesAsync();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackbar.Add(message, Severity.Error);
                }
            }
        }
    }

    private void ViewPermissions(string roleId)
    {
        _navigation.NavigateTo($"/role-permissions/{roleId}");
    }

    private void Cancel()
    {
        _navigation.NavigateTo("/");
    }
}
