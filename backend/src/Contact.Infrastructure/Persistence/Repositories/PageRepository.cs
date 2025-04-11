using Contact.Domain.Entities;
using Contact.Domain.Interfaces;
using Contact.Infrastructure.Persistence.Helper;

namespace Contact.Infrastructure.Persistence.Repositories;

public class PageRepository : GenericRepository<Page>, IPageRepository
{
    public PageRepository(IDapperHelper dapperHelper) : base(dapperHelper, "Pages")
    {
    }

    public async Task<Page> AddPage(Page page)
    {
        var query = $@"
            INSERT INTO ""Pages"" (""Name"", ""Url"", ""CreatedOn"", ""CreatedBy"")
            VALUES (@Name, @Url, @CreatedOn, @CreatedBy)
            RETURNING *;";
        return await _dapperHelper.Insert<Page>(query, page);
    }

    public async Task<Page> UpdatePage(Page page)
    {
        var query = $@"
            UPDATE ""Pages""
            SET ""Name"" = @Name,
                ""Url"" = @Url,
                ""UpdatedOn"" = @UpdatedOn,
                ""UpdatedBy"" = @UpdatedBy
            WHERE ""Id"" = @Id
            RETURNING *;";
        return await _dapperHelper.Update<Page>(query, page);
    }

    public async Task<IEnumerable<Page>> GetPages()
    {
        var query = $@"SELECT * FROM ""Pages"";";
        return await _dapperHelper.GetAll<Page>(query, null);
    }

    public async Task<bool> DeletePage(Guid id)
    {
        var query = $@"DELETE FROM ""Pages"" WHERE ""Id"" = @Id;";
        var result = await _dapperHelper.Execute(query, new { Id = id });
        return result != 0;
    }
}
