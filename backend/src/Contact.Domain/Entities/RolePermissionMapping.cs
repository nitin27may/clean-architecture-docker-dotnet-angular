namespace Contact.Domain.Entities;

public class RolePermissionMapping
{
    public Guid Id { get; set; }
    public Guid RoleId { get; set; }
    public string RoleName { get; set; }
    public Guid PageId { get; set; }
    public string PageName { get; set; } 
      public int PageOrder { get; set; } 
    public string PageUrl { get; set; }
    public Guid OperationId { get; set; }
    public string OperationName { get; set; }
    public Guid PermissionId { get; set; }
}
