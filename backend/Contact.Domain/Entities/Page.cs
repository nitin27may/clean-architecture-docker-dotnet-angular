namespace Contact.Domain.Entities;

public class Page : BaseEntity
{
    public required string Name { get; set; }
    public required string Url { get; set; }
    public int PageOrder { get; set; } = 500; // Default value in the middle for easy ordering
}
