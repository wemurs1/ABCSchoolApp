using ABCShared.Library.Models.Requests.Tenancy;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ABCSchoolApp.Pages.Tenancy;

public partial class CreateTenant
{
    [CascadingParameter] private IMudDialogInstance _dialogInstance { get; set; } = default!;
    private CreateTenantRequest CreateTenantRequest { get; set; } = new()
    { Identifier = string.Empty, Name = string.Empty, FirstName = string.Empty, LastName = string.Empty, Email = string.Empty };
    private MudForm _form = default!;
    private MudDatePicker _datePicker = default!;

    private DateTime? ValidUpToPicker
    {
        get => CreateTenantRequest.ValidUpTo == default ? null : CreateTenantRequest.ValidUpTo;
        set
        {
            if (value.HasValue)
            {
                CreateTenantRequest.ValidUpTo = value.Value;
            }
        }
    }

    private async Task SaveTenantAsync()
    {
        var result = await _tenantService.CreateAsync(CreateTenantRequest);
        if (result.IsSuccessful)
        {
            _snackbar.Add(result.Messages[0], Severity.Success);
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
        _dialogInstance.Close();
    }
}
