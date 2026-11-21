using Contact.Application.Abstractions.Messaging;
using Contact.Application.Interfaces;

namespace Contact.Application.UseCases.ContactPerson;

/// <summary>
/// Creates a contact. Delegates to the existing <see cref="IContactPersonService"/> rather than
/// reimplementing its logic (ADR 0002: convert the boundary first, leave the services intact).
/// </summary>
public sealed class CreateContactPersonHandler(IContactPersonService contactPersonService)
    : IRequestHandler<CreateContactPerson, ContactPersonResponse>
{
    public Task<ContactPersonResponse> Handle(CreateContactPerson request, CancellationToken cancellationToken)
        => contactPersonService.Add(request);
}
