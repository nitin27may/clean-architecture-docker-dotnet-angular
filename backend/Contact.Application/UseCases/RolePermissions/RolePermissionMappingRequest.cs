namespace Contact.Application.UseCases.RolePermissions;

public class RolePermissionMappingRequest
{
    public Guid RoleId { get; set; }
    public List<PageOperationRequest> Permissions { get; set; } = new();
}

public class PageOperationRequest
{
    public Guid PageId { get; set; }
    public List<Guid> OperationIds { get; set; } = new();
}