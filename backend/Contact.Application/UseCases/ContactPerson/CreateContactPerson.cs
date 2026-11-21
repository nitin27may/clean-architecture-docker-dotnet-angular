using Contact.Application.Abstractions.Messaging;

namespace Contact.Application.UseCases.ContactPerson;

/// <summary>Request body for POST /api/ContactPerson, and the command sent through <see cref="ISender"/>.</summary>
public class CreateContactPerson : IRequest<ContactPersonResponse>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public required string Mobile { get; set; }
    public required string Email { get; set; }
    public required string City { get; set; }
    public required string PostalCode { get; set; }
}
