namespace Contact.Application.UseCases.RolePermissions;

public class RolePermissionResponse
{
    public Guid Id { get; set; }
    public Guid PermissionId { get; set; }
    public Guid RoleId { get; set; }
    public string RoleName { get; set; }
    public string PageName { get; set; } 
    public int PageOrder { get; set; }
    public string PageUrl { get; set; }
    public string OperationName { get; set; }

}
