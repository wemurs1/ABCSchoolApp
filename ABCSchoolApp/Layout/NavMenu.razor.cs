using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace ABCSchoolApp.Layout;

public partial class NavMenu
{
    [CascadingParameter] protected Task<AuthenticationState> AuthState { get; set; } = default!;
    [Inject] protected IAuthorizationService AuthorizationService { get; set; } = default!;

    private bool _canViewTenants = false;
    private bool _canViewUsers = false;
    private bool _canViewRoles = false;
    private bool _canViewSchools = false;

    protected override async Task OnParametersSetAsync()
    {
        var user = (await AuthState).User;

        _canViewTenants = await AuthorizationService.HasPermissionAsync(user, SchoolFeature.Tenants, SchoolAction.Read);
        _canViewUsers = await AuthorizationService.HasPermissionAsync(user, SchoolFeature.Users, SchoolAction.Read);
        _canViewRoles = await AuthorizationService.HasPermissionAsync(user, SchoolFeature.Roles, SchoolAction.Read);
        _canViewSchools =await AuthorizationService.HasPermissionAsync(user, SchoolFeature.Schools, SchoolAction.Read);
    }
}