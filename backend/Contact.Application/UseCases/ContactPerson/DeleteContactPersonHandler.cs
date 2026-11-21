using Contact.Application.Abstractions.Messaging;
using Contact.Application.Interfaces;

namespace Contact.Application.UseCases.ContactPerson;

public sealed class DeleteContactPersonHandler(IContactPersonService contactPersonService)
    : IRequestHandler<DeleteContactPerson, bool>
{
    public Task<bool> Handle(DeleteContactPerson request, CancellationToken cancellationToken)
        => contactPersonService.Delete(request.Id);
}
