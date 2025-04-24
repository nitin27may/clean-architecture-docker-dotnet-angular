namespace Contact.Domain.Entities;

public class ContactPerson : BaseEntity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required DateTime DateOfBirth { get; set; }
    public required string Mobile { get; set; }
    public required string Email { get; set; }
    public required string City { get; set; }
    public required string PostalCode { get; set; }
}
