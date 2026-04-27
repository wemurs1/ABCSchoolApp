namespace ABCSchoolApp.Pages.Identity;

public partial class CreateRole
{
    public CreateRoleRequest CreateRoleRequest { get; set; } = new() { Name = string.Empty, Description = string.Empty };

    [CascadingParameter] private IMudDialogInstance _dialogInstance { get; set; } = default!;

    private MudForm _form = default!;

      private async Task SubmitCreateRoleAsync()
    {
        var result = await _roleService.CreateAsync(CreateRoleRequest);
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
}
