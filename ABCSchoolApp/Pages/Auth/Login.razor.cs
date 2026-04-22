using ABCApp.Infrastructure.Models;

namespace ABCSchoolApp.Pages.Auth;

public partial class Login
{
    private LoginRequest _loginRequest = new() { Username = string.Empty, Password = string.Empty, Tenant = string.Empty };

    private InputType _inputType = InputType.Password;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
    private bool _isPasswordVisible = false;
    private MudForm _form = default!;

    protected override async Task OnInitializedAsync()
    {
        var state = await _applicationStateProvider.GetAuthenticationStateAsync();
        if (state.User.Identity?.IsAuthenticated is true) _navigation.NavigateTo("/");
    }

    private async Task SubmitAsync()
    {
        var result = await _tokenService.LoginAsync(tenant: _loginRequest.Tenant, request: _loginRequest);
        if (result.IsSuccessful)
        {
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

    void TogglePasswordVisibility()
    {
        if (_isPasswordVisible)
        {
            _isPasswordVisible = false;
            _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
            _inputType = InputType.Password;
        }
        else
        {
            _isPasswordVisible = true;
            _passwordInputIcon = Icons.Material.Filled.Visibility;
            _inputType = InputType.Text;
        }
    }

    private void FillRootAdminCredentialsDuringDevelopment()
    {
        _loginRequest.Tenant = "root";
        _loginRequest.Username = "admin@abcschool.com";
        _loginRequest.Password = "P@ssw0rd@123";
    }
}
