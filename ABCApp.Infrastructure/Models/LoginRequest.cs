namespace ABCApp.Infrastructure.Models;

public class LoginRequest : TokenRequest
{
    public required string Tenant { get; set; }
}
