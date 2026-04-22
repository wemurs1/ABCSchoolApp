namespace ABCSchoolApp.Components;

public partial class Confirmation
{
    [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public required string Title { get; set; }
    [Parameter] public required string Message { get; set; }
    [Parameter] public required string ButtonText { get; set; }
    [Parameter] public Color Color { get; set; }
    [Parameter] public required string InputIcon { get; set; }

    private void Confirmed()
    {
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }
}
