namespace ABCApp.Infrastructure;

public class ApiSettings
{
    public required string BaseApiUrl { get; set; }
    public required TokenEndpoints TokenEndpoints { get; set; }
    public required UserEndpoints UserEndpoints { get; set; }
    public required TenantsEndpoints TenantsEndpoints { get; set; }
}

public class TokenEndpoints
{
    public required string Login { get; set; }
    public required string RefreshToken { get; set; }
}

public class UserEndpoints
{
    public required string Update { get; set; }
    public required string ResetPassword { get; set; }
}

public class TenantsEndpoints
{
    public required string Create { get; set; }
    public required string Upgrade { get; set; }
    public required string All { get; set; }
    public required string ById { get; set; }
    public required string Activate { get; set; }
    public required string Deactivate { get; set; }

    public string GetById(string tenantId)
    {
        return $"{ById}{tenantId}";
    }

    public string FullActivate(string tenantId)
    {
        return $"{Activate}{tenantId}/activate";
    }

    public string FullDeactvate(string tenantId)
    {
        return $"{Deactivate}{tenantId}/deactivate";
    }
}