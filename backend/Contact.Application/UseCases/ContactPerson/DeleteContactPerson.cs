using Contact.Application.Abstractions.Messaging;

namespace Contact.Application.UseCases.ContactPerson;

/// <summary>Deletes a contact. Responds with <c>false</c> when no contact has that id.</summary>
public sealed record DeleteContactPerson(Guid Id) : IRequest<bool>;
