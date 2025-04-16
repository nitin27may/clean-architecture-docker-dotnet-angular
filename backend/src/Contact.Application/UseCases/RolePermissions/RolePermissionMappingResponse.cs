namespace Contact.Application.UseCases.RolePermissions;

public class RolePermissionMappingResponse
{
    public Guid RoleId { get; set; }
    public string RoleName { get; set; }
    public List<PageOperationResponse> Pages { get; set; }
}

public class PageOperationResponse
{
    public Guid PageId { get; set; }
    public string PageName { get; set; }
    public List<OperationResponse> Operations { get; set; }
}

public class OperationResponse
{
    public Guid OperationId { get; set; }
    public string OperationName { get; set; }
    public bool IsSelected { get; set; }
}