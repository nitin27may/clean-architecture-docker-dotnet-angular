namespace Contact.Application.UseCases.Permissions;

public class CreatePermission
{
    public Guid PageId { get; set; }
    public Guid OperationId { get; set; }
    public Guid CreatedBy { get; set; }
}
