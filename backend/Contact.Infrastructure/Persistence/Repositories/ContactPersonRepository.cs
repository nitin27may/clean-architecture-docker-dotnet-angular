using Contact.Domain.Entities;
using Contact.Domain.Interfaces;
using Contact.Infrastructure.Persistence.Helper;

namespace Contact.Infrastructure.Persistence.Repositories;

public class ContactPersonRepository(IDapperHelper dapperHelper) 
    : GenericRepository<ContactPerson>(dapperHelper, "Contacts"), 
      IContactPersonRepository
{
    // Additional specific methods can be implemented here if needed
}
