namespace Contact.Domain.Entities;
public class Permission : BaseEntity
{
    public Guid PageId { get; set; }
    public Guid OperationId { get; set; }
    public required string Description { get; set; }

}
