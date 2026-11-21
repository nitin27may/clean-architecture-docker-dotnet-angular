namespace Contact.Application.UseCases.ContactPerson;

/// <summary>
/// The contact fields shared by <see cref="CreateContactPerson"/> and <see cref="UpdateContactPerson"/>,
/// so one set of validation rules (<see cref="ContactPersonDetailsValidator{T}"/>) covers both.
/// </summary>
public interface IContactPersonDetails
{
    string FirstName { get; }
    string LastName { get; }
    DateOnly DateOfBirth { get; }
    string Mobile { get; }
    string Email { get; }
    string City { get; }
    string PostalCode { get; }
}
