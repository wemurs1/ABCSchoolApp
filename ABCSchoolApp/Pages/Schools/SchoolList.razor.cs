using ABCShared.Library.Models.Responses.Schools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace ABCSchoolApp.Pages.Schools;

public partial class SchoolList
{
    [CascadingParameter] protected Task<AuthenticationState> AuthenticationState { get; set; } = default!;
    [Inject] protected IAuthorizationService AuthorizationService { get; set; } = default!;

    private bool _isLoading = true;

    private bool _canCreateSchools = false;
    private bool _canUpdateSchools = false;
    private bool _canDeleteSchools = false;

    private List<SchoolResponse> _schoolList = [];

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthenticationState).User;

        _canCreateSchools = await AuthorizationService.HasPermissionAsync(user, SchoolFeature.Schools, SchoolAction.Create);
        _canUpdateSchools = await AuthorizationService.HasPermissionAsync(user, SchoolFeature.Schools, SchoolAction.Update);
        _canDeleteSchools = await AuthorizationService.HasPermissionAsync(user, SchoolFeature.Schools, SchoolAction.Delete);

        await LoadSchoolsAsync();
        _isLoading = false;
    }

    private async Task LoadSchoolsAsync()
    {
        var result = await _schoolService.GetAllAsync();
        if (result.IsSuccessful)
        {
            _schoolList = result.Data;
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
        _navigation.NavigateTo("/");
    }
}
