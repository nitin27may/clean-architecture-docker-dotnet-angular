namespace Contact.Domain.Entities;

public class RolePermissionMapping : BaseEntity
{
    public string RoleName { get; set; }
    public string PageName { get; set; }
    public string PageUrl { get; set; }
    public string OperationName { get; set; }
}
