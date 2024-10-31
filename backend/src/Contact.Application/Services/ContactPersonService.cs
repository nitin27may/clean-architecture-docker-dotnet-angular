using AutoMapper;
using Contact.Application.Interfaces;
using Contact.Application.UseCases.ContactPerson;
using Contact.Domain.Entities;
using Contact.Domain.Interfaces;

namespace Contact.Application.Services;


public class ContactPersonService : GenericService<ContactPerson, ContactPersonResponse, CreateContactPerson, UpdateContactPerson>, IContactPersonService
{
    public ContactPersonService(IGenericRepository<ContactPerson> repository, IMapper mapper, IUnitOfWork unitOfWork)
        : base(repository, mapper, unitOfWork)
    {
    }

    // Additional methods specific to Contact Person Entity can go here if needed
}
