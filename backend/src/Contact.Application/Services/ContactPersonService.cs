using AutoMapper;
using Contact.Application.Interfaces;
using Contact.Application.UseCases.ContactPerson;
using Contact.Domain.Entities;
using Contact.Domain.Interfaces;

namespace Contact.Application.Services;

public class ContactPersonService(
    IGenericRepository<ContactPerson> repository, 
    IMapper mapper, 
    IUnitOfWork unitOfWork)
    : GenericService<ContactPerson, ContactPersonResponse, CreateContactPerson, UpdateContactPerson>(repository, mapper, unitOfWork), 
      IContactPersonService
{
    // Additional methods specific to Contact Person Entity can go here if needed
}
