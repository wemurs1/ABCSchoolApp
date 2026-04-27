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
        var user = (await AuthState).User;
        _canCreateUsers = await AuthorizationService.HasPermissionAsync(user, SchoolFeature.Users, SchoolAction.Create);
        _canViewRoles = await AuthorizationService.HasPermissionAsync(user, SchoolFeature.UserRoles, SchoolAction.Read);
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

    private void GoToRoles(string userId)
    {
        _navigation.NavigateTo($"/user-roles/{userId}");
    }

    private async Task ActivateOrDeactivativeAsync(UserResponse user)
    {
        if (user.IsActive)
        {
            // Deactivate
            var parameters = new DialogParameters
                {
                    { nameof(Confirmation.Title), "Deactivate User" },
                    { nameof(Confirmation.Message), $"Are you sure you want to Deactivate user: {user.FirstName} {user.LastName}?" },
                    { nameof(Confirmation.ButtonText), "Deactivate" },
                    {nameof(Confirmation.Color), Color.Error },
                    {nameof(Confirmation.InputIcon), Icons.Material.Filled.Person }
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
                var response = await _userService.ChangeUserStatusASync(new ChangeUserStatusRequest { UserId = user.Id, Activation = false });

                if (response.IsSuccessful)
                {
                    _snackbar.Add(response.Messages[0], Severity.Success);

                    await LoadUsersAsync();
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
        else
        {
            // Activate
            var parameters = new DialogParameters
                {
                    { nameof(Confirmation.Title), "Activate User" },
                    { nameof(Confirmation.Message), $"Are you sure you want to activate user: {user.FirstName} {user.LastName}?" },
                    { nameof(Confirmation.ButtonText), "Activate" },
                    {nameof(Confirmation.Color), Color.Primary },
                    {nameof(Confirmation.InputIcon), Icons.Material.Filled.Person }
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
                var response = await _userService.ChangeUserStatusASync(new ChangeUserStatusRequest { UserId = user.Id, Activation = true });

                if (response.IsSuccessful)
                {
                    _snackbar.Add(response.Messages[0], Severity.Success);

                    await LoadUsersAsync();
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
    }

    private void Cancel()
    {
        _navigation.NavigateTo("/");
    }
}
