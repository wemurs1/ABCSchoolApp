using MudBlazor;

namespace ABCSchoolApp.Pages.Identity;

public partial class PasswordReset
{
    private ChangePasswordRequest ChangePasswordRequest { get; set; } = new()
    { UserId = string.Empty, ConfirmNewPassword = string.Empty, CurrentPassword = string.Empty, NewPassword = string.Empty };

    private class PasswordVisibility
    {
        public bool Visible { get; set; } = false;
        public InputType InputType { get; set; } = InputType.Password;
        public string InputIcon { get; set; } = Icons.Material.Filled.VisibilityOff;
    }

    private PasswordVisibility _currentPasswordVisible = new();
    private PasswordVisibility _newPasswordVisible = new();
    private PasswordVisibility _confirmPasswordVisible = new();
    private MudForm _form = default!;

    protected async override Task OnInitializedAsync()
    {
        await SetCurrentUserDetails();
    }

    private async Task SetCurrentUserDetails()
    {
        var state = await _applicationStateProvider.GetAuthenticationStateAsync();
        ChangePasswordRequest.UserId = state.User.GetUserId()!;
    }

    private async Task ResetPasswordAsync()
    {
        var result = await _userService.ChangeUserPassword(ChangePasswordRequest);
        if (result.IsSuccessful)
        {
            await _tokenService.LogoutAsync();
            _snackbar.Add("You have changed your password. Login again", Severity.Info);
            _navigation.NavigateTo("/");
        }
        else
        {
            foreach (var message in result.Messages)
            {
                _snackbar.Add(message, Severity.Error);
            }
        }
    }

    private static void TogglePasswordVisibility(PasswordVisibility visibility)
    {
        if (visibility.Visible)
        {
            visibility.Visible = false;
            visibility.InputIcon = Icons.Material.Filled.VisibilityOff;
            visibility.InputType = InputType.Password;
        }
        else
        {
            visibility.Visible = true;
            visibility.InputIcon = Icons.Material.Filled.Visibility;
            visibility.InputType = InputType.Text;
        }
    }
}