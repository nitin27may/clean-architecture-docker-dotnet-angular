using Contact.Application.Abstractions.Messaging;

namespace Contact.Application.UseCases.ContactPerson;

/// <summary>Reads every contact.</summary>
public sealed record GetAllContactPersons : IRequest<IEnumerable<ContactPersonResponse>>;
