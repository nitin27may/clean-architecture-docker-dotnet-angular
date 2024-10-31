namespace Contact.Domain.Entities;

public class Page : BaseEntity
{
    public required string Name { get; set; }
    public required string Url { get; set; }
}
