namespace Contact.Application.UseCases.RolePermissions;

public class RolePermissionResponse
{
    public Guid PermissionId { get; set; }
    public string PageName { get; set; }
    public string OperationName { get; set; }
    public Guid RoleId { get; set; }
    public string RoleName { get; set; }
}
