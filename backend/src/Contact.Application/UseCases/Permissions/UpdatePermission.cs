namespace Contact.Application.UseCases.Permissions;

public class UpdatePermission
{
    public Guid PageId { get; set; }
    public Guid OperationId { get; set; }
    public Guid UpdatedBy { get; set; }
}
