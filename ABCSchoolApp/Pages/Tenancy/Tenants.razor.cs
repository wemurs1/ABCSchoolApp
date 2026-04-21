using ABCShared.Library.Models.Responses.Tenancy;
using MudBlazor;

namespace ABCSchoolApp.Pages.Tenancy;

public partial class Tenants
{
    private List<TenantResponse> TenantList { get; set; } = [];
    private bool _isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        await LoadTenantsAsync();
        _isLoading = false;
    }

    private async Task LoadTenantsAsync()
    {
        var result = await _tenantService.GetAllAsync();
        if (result.IsSuccessful)
        {
            TenantList = result.Data;
        }
        else
        {
            foreach (var message in result.Messages)
            {
                _snackbar.Add(message, MudBlazor.Severity.Error);
            }
        }
    }

    private void ReturnClicked()
    {
        _navigation.NavigateTo("/");
    }

    private async Task OnBoardNewTenantAsync()
    {
        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            BackdropClick = false
        };
        var dialog = await _dialogService.ShowAsync<CreateTenant>("Onboard New Tenant", options);
        var result = await dialog.Result ?? throw new Exception("result is null");
        if (!result.Canceled)
        {
            await LoadTenantsAsync();
        }
    }
}
