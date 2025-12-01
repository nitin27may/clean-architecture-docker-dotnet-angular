namespace Contact.Domain.Entities;

public class Operation : BaseEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }
}
