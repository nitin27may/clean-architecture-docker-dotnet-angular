using Contact.Domain.Entities;
using Contact.Domain.Interfaces;
using Contact.Infrastructure.Persistence.Helper;

namespace Contact.Infrastructure.Persistence.Repositories;

public class ContactPersonRepository : GenericRepository<ContactPerson>, IContactPersonRepository
{
    public ContactPersonRepository(IDapperHelper dapperHelper) : base(dapperHelper, "Contacts") 
    { 
    }
}
