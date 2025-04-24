using Contact.Domain.Entities;
using Contact.Domain.Interfaces;
using Contact.Infrastructure.Persistence.Helper;
using System.Data;

namespace Contact.Infrastructure.Persistence.Repositories;

public class GenericRepository<T>(IDapperHelper dapperHelper, string tableName) : IGenericRepository<T> 
    where T : BaseEntity
{
    public async Task<IEnumerable<T>> FindAll(IDbTransaction? transaction = null)
    {
        var query = $@"SELECT * FROM ""{tableName}"";";
        return await dapperHelper.GetAll<T>(query, null, CommandType.Text, transaction);
    }

    public async Task<T> FindByID(Guid id, IDbTransaction? transaction = null)
    {
        var query = $@"SELECT * FROM ""{tableName}"" WHERE ""Id"" = @Id;";
        return await dapperHelper.Get<T>(query, new { Id = id }, CommandType.Text, transaction);
    }

    public async Task<IEnumerable<T>> Find(string query, object? parameters = null, IDbTransaction? transaction = null) =>
        await dapperHelper.GetAll<T>(query, parameters, CommandType.Text, transaction);

    public async Task<T> Add(T entity, IDbTransaction? transaction = null)
    {
        var query = GenerateInsertQuery();
        return await dapperHelper.Insert<T>(query, entity, CommandType.Text, transaction);
    }

    public async Task<T> Update(T entity, IDbTransaction? transaction = null)
    {
        var query = GenerateUpdateQuery();
        return await dapperHelper.Update<T>(query, entity, CommandType.Text, transaction);
    }

    public async Task<bool> Delete(Guid id, IDbTransaction? transaction = null)
    {
        var query = $@"DELETE FROM ""{tableName}"" WHERE ""Id"" = @Id;";
        var result = await dapperHelper.Execute(query, new { Id = id }, CommandType.Text, transaction);
        return result != 0;
    }

    private string GenerateInsertQuery()
    {
        var properties = typeof(T).GetProperties()
            .Where(p => p.Name != "Id" && p.Name != "UpdatedOn" && p.Name != "UpdatedBy")
            .Select(p => p.Name);

        var columns = string.Join(", ", properties.Select(p => $@"""{p}"""));
        var values = string.Join(", ", properties.Select(p => $"@{p}"));

        return $@"
            INSERT INTO ""{tableName}"" ({columns})
            VALUES ({values})
            RETURNING *;";
    }

    private string GenerateUpdateQuery()
    {
        var properties = typeof(T).GetProperties()
            .Where(p => p.Name != "Id" && p.Name != "CreatedOn" && p.Name != "CreatedBy")
            .Select(p => $@"""{p.Name}"" = @{p.Name}");

        var setClause = string.Join(", ", properties);

        return $@"
            UPDATE ""{tableName}""
            SET {setClause}
            WHERE ""Id"" = @Id
            RETURNING *;";
    }
}
