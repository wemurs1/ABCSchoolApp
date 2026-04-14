namespace ABCSchoolApp.Components;

public partial class CurrentUserCard
{
    private string? Firstname { get; set; }
    private string? Lastname { get; set; }
    private char? FirstLetterOfFirstName { get; set; }
    private string? Email { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var state = await _applicationStateProvider.GetAuthenticationStateAsync();
        var user = state.User;
        Firstname = user.GetFirstName();
        Lastname = user.GetLastName();
        Email = user.GetEmail();
        FirstLetterOfFirstName = Firstname?[0];
    }
}
