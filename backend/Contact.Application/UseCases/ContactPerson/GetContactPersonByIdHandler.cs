using Contact.Application.Abstractions.Messaging;
using Contact.Application.Interfaces;

namespace Contact.Application.UseCases.ContactPerson;

public sealed class GetContactPersonByIdHandler(IContactPersonService contactPersonService)
    : IRequestHandler<GetContactPersonById, ContactPersonResponse?>
{
    public async Task<ContactPersonResponse?> Handle(GetContactPersonById request, CancellationToken cancellationToken)
        => await contactPersonService.FindByID(request.Id);
}
