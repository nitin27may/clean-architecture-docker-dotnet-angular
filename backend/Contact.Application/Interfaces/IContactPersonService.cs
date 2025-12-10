using Contact.Application.UseCases.ContactPerson;
using Contact.Domain.Entities;

namespace Contact.Application.Interfaces;


public interface IContactPersonService : IGenericService<ContactPerson, ContactPersonResponse, CreateContactPerson, UpdateContactPerson>
{
  
}
