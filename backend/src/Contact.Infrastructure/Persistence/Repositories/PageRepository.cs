using Contact.Domain.Entities;
using Contact.Domain.Interfaces;
using Contact.Infrastructure.Persistence.Helper;
using System.Data;

namespace Contact.Infrastructure.Persistence.Repositories;

public class PageRepository(IDapperHelper dapperHelper) 
    : GenericRepository<Page>(dapperHelper, "Pages"), 
      IPageRepository
{
    public async Task<Page> AddPage(Page page, IDbTransaction? transaction = null)
    {
        var query = $@"
            INSERT INTO ""Pages"" (""Name"", ""Url"", ""CreatedOn"", ""CreatedBy"")
            VALUES (@Name, @Url, @CreatedOn, @CreatedBy)
            RETURNING *;";
        return await dapperHelper.Insert<Page>(query, page, CommandType.Text, transaction);
    }

    public async Task<Page> UpdatePage(Page page, IDbTransaction? transaction = null)
    {
        var query = $@"
            UPDATE ""Pages""
            SET ""Name"" = @Name,
                ""Url"" = @Url,
                ""UpdatedOn"" = @UpdatedOn,
                ""UpdatedBy"" = @UpdatedBy
            WHERE ""Id"" = @Id
            RETURNING *;";
        return await dapperHelper.Update<Page>(query, page, CommandType.Text, transaction);
    }

    public async Task<IEnumerable<Page>> GetPages(IDbTransaction? transaction = null)
    {
        var query = $@"SELECT * FROM ""Pages"";";
        return await dapperHelper.GetAll<Page>(query, null, CommandType.Text, transaction);
    }

    public async Task<bool> DeletePage(Guid id, IDbTransaction? transaction = null)
    {
        var query = $@"DELETE FROM ""Pages"" WHERE ""Id"" = @Id;";
        var result = await dapperHelper.Execute(query, new { Id = id }, CommandType.Text, transaction);
        return result != 0;
    }
}
