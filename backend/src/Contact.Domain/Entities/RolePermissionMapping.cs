namespace Contact.Domain.Entities;

public class RolePermissionMapping
{
    public Guid RoleId { get; set; }
    public string RoleName { get; set; }
    public Guid PageId { get; set; }
    public string PageName { get; set; }
    public Guid OperationId { get; set; }
    public string OperationName { get; set; }
    public Guid PermissionId { get; set; }
}
