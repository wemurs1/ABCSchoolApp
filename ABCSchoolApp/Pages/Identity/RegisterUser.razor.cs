namespace ABCSchoolApp.Pages.Identity;

public partial class RegisterUser
{
    public CreateUserRequest CreateUserRequest { get; set; } = new()
    {
        FirstName = string.Empty,
        LastName = string.Empty,
        Email = string.Empty,
        Password = string.Empty,
        ConfirmPassword = String.Empty,
        PhoneNumber = string.Empty
    };

    [CascadingParameter] private IMudDialogInstance _dialogInstance { get; set; } = default!;

    private InputType _passwordInputType = InputType.Password;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
    private bool _isPasswordVisible;

    private InputType _confirmPasswordInputType = InputType.Password;
    private string _confirmPasswordInputIcon = Icons.Material.Filled.VisibilityOff;
    private bool _isConfirmPasswordVisible;

    private MudForm _form = default!;

    private async Task SubmitUserRegistrationAsync()
    {
        var result = await _userService.RegisterUserAsync(CreateUserRequest);
        if (result.IsSuccessful)
        {
            _snackbar.Add(result.Messages[0], Severity.Normal);
            _dialogInstance.Close(DialogResult.Ok(true));
        }
        else
        {
            foreach (var message in result.Messages)
            {
                _snackbar.Add(message, Severity.Error);
            }
        }
    }

    private void CancelDialog()
    {
        _dialogInstance.Cancel();
    }

    void TogglePasswordVisibility()
    {
        if (_isPasswordVisible)
        {
            _isPasswordVisible = false;
            _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
            _passwordInputType = InputType.Password;
        }
        else
        {
            _isPasswordVisible = true;
            _passwordInputIcon = Icons.Material.Filled.Visibility;
            _passwordInputType = InputType.Text;
        }
    }

    void ToggleConfirmPasswordVisibility()
    {
        if (_isConfirmPasswordVisible)
        {
            _isConfirmPasswordVisible = false;
            _confirmPasswordInputIcon = Icons.Material.Filled.VisibilityOff;
            _confirmPasswordInputType = InputType.Password;
        }
        else
        {
            _isConfirmPasswordVisible = true;
            _confirmPasswordInputIcon = Icons.Material.Filled.Visibility;
            _confirmPasswordInputType = InputType.Text;
        }
    }
}
