namespace ABCSchoolApp.Components;

public partial class NotAuthorized
{
    [Parameter] public string? Message { get; set; }

    private void GoHome() => _navigation.NavigateTo("/");
}
