using Contact.Application.Abstractions.Messaging;
using Contact.Application.Interfaces;

namespace Contact.Application.UseCases.ContactPerson;

public sealed class GetAllContactPersonsHandler(IContactPersonService contactPersonService)
    : IRequestHandler<GetAllContactPersons, IEnumerable<ContactPersonResponse>>
{
    public Task<IEnumerable<ContactPersonResponse>> Handle(GetAllContactPersons request, CancellationToken cancellationToken)
        => contactPersonService.FindAll();
}
