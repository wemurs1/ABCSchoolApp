namespace ABCApp.Infrastructure;

public class ApiSettings
{
    public required string BaseApiUrl { get; set; }
    public required TokenEndpoints TokenEndpoints { get; set; }
    public required UserEndpoints UserEndpoints { get; set; }
    public required TenantsEndpoints TenantsEndpoints { get; set; }
    public required RoleEndpoints RoleEndpoints { get; set; }
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
    public required string All { get; set; }
    public required string ById { get; set; }
    public required string Register { get; set; }
    public required string RolesById { get; set; }
    public required string UpdateRoles { get; set; }
    public required string UpdateStatus { get; set; }

    public string GetById(string userId) => $"{ById}{userId}";
    public string GetRolesById(string userId) => $"{RolesById}{userId}";
    public string UpdateRolesById(string userId) => $"{UpdateRoles}{userId}";
}

public class RoleEndpoints
{
    public required string Create { get; set; }
    public required string Update { get; set; }
    public required string ById { get; set; }
    public required string ByIdPartial { get; set; }
    public required string All { get; set; }
    public required string Delete { get; set; }
    public required string UpdatePermission { get; set; }

    public string GetById(string roleId) => $"{ById}{roleId}";
    public string GetByIdPartial(string roleId) => $"{ByIdPartial}{roleId}";
    public string DeleteRole(string roleId) => $"{Delete}{roleId}";
}

public class TenantsEndpoints
{
    public required string Create { get; set; }
    public required string Upgrade { get; set; }
    public required string All { get; set; }
    public required string ById { get; set; }
    public required string Activate { get; set; }
    public required string Deactivate { get; set; }

    public string GetById(string tenantId) => $"{ById}{tenantId}";
    public string FullActivate(string tenantId) => $"{Activate}{tenantId}/activate";
    public string FullDeactivate(string tenantId) => $"{Deactivate}{tenantId}/deactivate";
}