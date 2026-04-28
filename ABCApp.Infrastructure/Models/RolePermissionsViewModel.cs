namespace ABCApp.Infrastructure.Models;

public record RolePermissionsViewModel : SchoolPermission
{
    public RolePermissionsViewModel(string Action, string Feature, string Description, string Group, bool IsBasic = false, bool IsRoot = false)
        : base(Action, Feature, Description, Group, IsBasic, IsRoot) { }

    public bool IsSelected { get; set; }
}
