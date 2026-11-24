using Contact.Application.Abstractions.Messaging;

namespace Contact.Application.UseCases.ContactPerson;

/// <summary>
/// Request body for PUT /api/ContactPerson/{id}, and the command sent through <see cref="ISender"/>.
/// Responds with <c>null</c> when no contact has that id.
/// </summary>
public class UpdateContactPerson : IRequest<ContactPersonResponse?>, IContactPersonDetails
{
    /// <summary>Set from the route by the controller; a value in the request body is ignored.</summary>
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public required string Mobile { get; set; }
    public required string Email { get; set; }
    public required string City { get; set; }
    public required string PostalCode { get; set; }
}
