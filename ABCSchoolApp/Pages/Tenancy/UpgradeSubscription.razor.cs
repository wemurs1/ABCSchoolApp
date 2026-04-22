namespace ABCSchoolApp.Pages.Tenancy;

public partial class UpgradeSubscription
{
    [CascadingParameter] private IMudDialogInstance _dialogInstance { get; set; } = default!;
    [Parameter] public UpdateTenantSubscriptionRequest SubscriptionRequest { get; set; } = default!;

    private MudForm _form = default!;
    private DateTime? NewExpiryDatePicker
    {
        get => SubscriptionRequest.NewExpiryDate == default ? null : SubscriptionRequest.NewExpiryDate;
        set
        {
            if (value.HasValue)
            {
                SubscriptionRequest.NewExpiryDate = value.Value;
            }
        }
    }

    private async Task UpgradeSubscriptionAsync()
    {
        var result = await _tenantService.UpgradeSubscriptionAsync(SubscriptionRequest);
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
