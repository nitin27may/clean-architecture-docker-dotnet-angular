namespace Contact.Application.UseCases.Permissions;

public class UpdatePermission
{
       public Guid Id { get; set; }
    public Guid PageId { get; set; }
    public Guid OperationId { get; set; }
    public Guid UpdatedBy { get; set; }
}
