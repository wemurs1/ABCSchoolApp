using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ABCSchoolApp.Components;

public partial class Logout
{
    [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public required string Title { get; set; }
    [Parameter] public required string ConfirmationMessage { get; set; }
    [Parameter] public required string ButtonText { get; set; }
    [Parameter] public Color Color { get; set; }

    private async Task LogoutAsync()
    {
        var result = await _tokenService.LogoutAsync();
        if (result.IsSuccessful)
        {
            _navigation.NavigateTo("/");
            MudDialog.Close(DialogResult.Ok(true));
        }
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }
}