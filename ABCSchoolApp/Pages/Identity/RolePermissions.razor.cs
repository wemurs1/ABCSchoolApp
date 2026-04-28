using System.Security.Claims;
using ABCApp.Infrastructure.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace ABCSchoolApp.Pages.Identity;

public partial class RolePermissions
{
    [CascadingParameter] protected Task<AuthenticationState> AuthenticationState { get; set; } = default!;
    [Inject] protected IAuthorizationService AuthorizationService { get; set; } = default!;

    [Parameter] public string RoleId { get; set; } = string.Empty;

    private bool _isLoading = true;
    private string _title = string.Empty;
    private string _description = string.Empty;

    private RoleResponse _roleClaimResponse = new() { Id = string.Empty, Name = string.Empty, Description = string.Empty };
    private Dictionary<string, List<RolePermissionsViewModel>> RoleClaimsGroup { get; set; } = [];

    private bool _canUpdateRolePermissions;

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthenticationState).User;
        _canUpdateRolePermissions = await AuthorizationService.HasPermissionAsync(user, SchoolFeature.RoleClaims, SchoolAction.Update);

        await GetRolePermissionsAsync(user);
        _isLoading = false;
    }

    private async Task GetRolePermissionsAsync(ClaimsPrincipal user)
    {
        var result = await _roleService.GetRoleWithPermissionsAsync(RoleId);
        if (result.IsSuccessful)
        {
            _roleClaimResponse = result.Data;
            _title = "Permission Management";
            _description = string.Format("Manage {0}'s Permissions", _roleClaimResponse.Name);

            var permissions = user.GetTenant() == MultitenancyConstants.RootId ? SchoolPermissions.All : SchoolPermissions.Admin;

            RoleClaimsGroup = permissions.GroupBy(p => p.Feature).ToDictionary(g => g.Key, g => g.Select(p =>
            {
                var permission = new RolePermissionsViewModel(
                    Action: p.Action,
                    Feature: p.Feature,
                    Description: p.Description,
                    Group: p.Group,
                    IsBasic: p.IsBasic,
                    IsRoot: p.IsRoot
                );
                permission.IsSelected = _roleClaimResponse.Permissions.Contains(permission.Name);
                return permission;
            }).ToList());
        }
        else
        {
            foreach (var message in result.Messages)
            {
                _snackbar.Add(message, Severity.Error);
            }
            _navigation.NavigateTo("/roles");
        }
    }

    private Color GetGroupBadgeColor(int selectedCount, int all)
    {
        if (selectedCount == 0) return Color.Error;
        if (selectedCount == all) return Color.Success;
        return Color.Warning;
    }

    private async Task UpdateRolePermissionsAsync()
    {
        var allPermissions = RoleClaimsGroup.Values.SelectMany(permissions => permissions);

        var request = new UpdateRolePermissionsRequest
        {
            RoleId = RoleId,
            NewPermissions = allPermissions.Where(rpvm => rpvm.IsSelected).Select(permission => permission.Name).ToList()
        };
        var result = await _roleService.UpdatePermissionsAsync(request);
        if (result.IsSuccessful)
        {
            _snackbar.Add(result.Messages[0], Severity.Success);
            _navigation.NavigateTo("/roles");
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
        _navigation.NavigateTo("/roles");
    }
}
