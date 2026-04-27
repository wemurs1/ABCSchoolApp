namespace ABCSchoolApp.Pages.Identity;

public partial class UpdateRole
{
    [Parameter] public string RoleId { get; set; } = string.Empty;

    private RoleResponse _roleResponse = new()
    {
        Id = string.Empty,
        Name = string.Empty,
        Description = string.Empty
    };
    private bool _isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        await LoadRoleAsync();
        _isLoading = false;
    }

    private async Task LoadRoleAsync()
    {
        var response = await _roleService.GetRoleWithoutPermissionsAsync(RoleId);
        Console.WriteLine(response);
        if (response.IsSuccessful)
        {
            _roleResponse = response.Data;
        }
    }

    private async Task UpdateRoleDetailsAsync()
    {
        var request = new UpdateRoleRequest
        {
            Id = _roleResponse.Id,
            Name = _roleResponse.Name,
            Description = _roleResponse.Description
        };
        var result = await _roleService.UpdateAsync(request);
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
