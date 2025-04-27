namespace Contact.Application.UseCases.Permissions;

public class PermissionResponse
{
    public Guid Id { get; set; }
    public Guid PageId { get; set; }
    public string PageName { get; set; }
    public Guid OperationId { get; set; }
    public string OperationName { get; set; }
    public string Description { get; set; }
}