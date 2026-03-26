namespace ABCSchoolApp.Infrastructure;

public class ApiSettings
{
    public required string BaseApiUrl { get; set; }
    public required TokenEndpoints TokenEndpoints { get; set; } 
}

public class TokenEndpoints
{
    public required string Login { get; set; }
    public required string RefreshToken { get; set; }
}
