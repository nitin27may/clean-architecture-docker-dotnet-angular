using Contact.Application.Abstractions.Messaging;
using Contact.Application.Interfaces;

namespace Contact.Application.UseCases.ContactPerson;

public sealed class UpdateContactPersonHandler(IContactPersonService contactPersonService)
    : IRequestHandler<UpdateContactPerson, ContactPersonResponse?>
{
    public async Task<ContactPersonResponse?> Handle(UpdateContactPerson request, CancellationToken cancellationToken)
    {
        var existing = await contactPersonService.FindByID(request.Id);
        if (existing is null)
        {
            return null;
        }

        return await contactPersonService.Update(request);
    }
}
