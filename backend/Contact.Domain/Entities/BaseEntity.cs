namespace Contact.Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public required DateTimeOffset CreatedOn { get; set; }
    public required Guid CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public Guid? UpdatedBy { get; set; }
}
