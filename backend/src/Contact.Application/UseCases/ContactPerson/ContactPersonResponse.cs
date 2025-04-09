namespace Contact.Application.UseCases.ContactPerson;

public class ContactPersonResponse
{
    public Guid Id { get; set; }

    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public required string Mobile { get; set; }
    public required string Email { get; set; }
    public required string City { get; set; }
    public required string PostalCode { get; set; }
}
