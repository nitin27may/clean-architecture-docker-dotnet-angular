using Contact.Application.Abstractions.Messaging;

namespace Contact.Application.UseCases.ContactPerson;

/// <summary>Reads one contact. Responds with <c>null</c> when no contact has that id.</summary>
public sealed record GetContactPersonById(Guid Id) : IRequest<ContactPersonResponse?>;
