using Contact.Domain.Entities;
using Contact.Domain.Interfaces;
using Contact.Infrastructure.Persistence.Helper;

namespace Contact.Infrastructure.Persistence.Repositories
{
    public class PageOperationMappingRepository : GenericRepository<PageOperationMapping>, IGenericRepository<PageOperationMapping>
    {
        public PageOperationMappingRepository(IDapperHelper dapperHelper) : base(dapperHelper, "Permissions")
        {
        }

        // Add any specific methods for PageOperationMapping if needed
    }
}
