using ABCShared.Library.Models.Responses.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace ABCSchoolApp.Pages.Identity;

public partial class Users
{
    [CascadingParameter] protected Task<AuthenticationState> AuthState { get; set; } = default!;
    [Inject] protected IAuthorizationService AuthorizationService { get; set; } = default!;
    private List<UserResponse> _userList = [];
    private bool _isLoading = true;
    private bool _canCreateUsers = false;
    private bool _canViewRoles = false;

    protected override async Task OnInitializedAsync()
    {
        await LoadUsersAsync();
        _isLoading = false;
    }

    private async Task LoadUsersAsync()
    {
        var result = await _userService.GetUsersAsync();
        if (result.IsSuccessful)
        {
            _userList = result.Data;
        }
        else
        {
            foreach (var message in result.Messages)
            {
                _snackbar.Add(message, Severity.Error);
            }
        }
    }


    private async Task InvokeUserRegistrationDialog()
    {
        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Small,
            BackdropClick = false,
            FullWidth = true
        };

        var dialog = await _dialogService.ShowAsync<RegisterUser>(title: null, options: options);
        var result = (await dialog.Result)!;
        if (!result.Canceled)
        {
            await LoadUsersAsync();
        }
    }


    private void Cancel()
    {
        _navigation.NavigateTo("/");
    }
}
