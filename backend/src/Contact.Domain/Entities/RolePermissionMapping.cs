namespace Contact.Domain.Entities;

public class RolePermissionMapping : BaseEntity
{
    public Guid RoleId { get; set; }
    public string RoleName { get; set; }
    public Guid PermissionId { get; set; }
    public string PageName { get; set; }
    public string PageUrl { get; set; }
    public string OperationName { get; set; }
}
