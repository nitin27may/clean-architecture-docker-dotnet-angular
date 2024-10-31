using Contact.Domain.Entities;
using Contact.Domain.Interfaces;
using Contact.Infrastructure.Persistence.Helper;
using System.Data;

namespace Contact.Infrastructure.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly IDapperHelper _dapperHelper;
    protected readonly string _tableName;

    public GenericRepository(IDapperHelper dapperHelper, string tableName)
    {
        _dapperHelper = dapperHelper;
        _tableName = tableName;
    }

    public async Task<IEnumerable<T>> FindAll()
    {
        var query = $"SELECT * FROM {_tableName}";
        return await _dapperHelper.GetAll<T>(query, null);
    }

    public async Task<IEnumerable<T>> FindAll(Guid societyId)
    {
        var query = $"SELECT * FROM {_tableName} WHERE SocietyId = @SocietyId";
        return await _dapperHelper.GetAll<T>(query, new { SocietyId = societyId });
    }

    public async Task<T> FindByID(Guid id)
    {
        var query = $"SELECT * FROM {_tableName} WHERE Id = @Id";
        return await _dapperHelper.Get<T>(query, new { Id = id });
    }

    public async Task<IEnumerable<T>> Find(string query, object? parameters = null)
    {
        return await _dapperHelper.GetAll<T>(query, parameters);
    }

    public async Task<T> Add(T entity, System.Data.IDbTransaction? transaction = null)
    {
        var query = GenerateInsertQuery();
        return await _dapperHelper.Insert<T>(query, entity, CommandType.Text, transaction);
    }

    public async Task<T> Update(T entity, System.Data.IDbTransaction? transaction = null)
    {
        var query = GenerateUpdateQuery();
        return await _dapperHelper.Update<T>(query, entity, CommandType.Text, transaction);
    }

    public async Task<bool> Delete(Guid id)
    {
        var query = $"DELETE FROM {_tableName} WHERE Id = @Id";
        var result = await _dapperHelper.Execute(query, new { Id = id });
        if (result != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private string GenerateInsertQuery()
    {
        var properties = typeof(T).GetProperties()
            .Where(p => p.Name != "Id" && p.Name != "UpdatedOn" && p.Name != "UpdatedBy")
            .Select(p => p.Name);

        var columns = string.Join(", ", properties);
        var values = string.Join(", ", properties.Select(p => $"@{p}"));

        return $@"
                    INSERT INTO {_tableName} ({columns})
                    OUTPUT INSERTED.*
                    VALUES ({values})
                ";
    }

    private string GenerateUpdateQuery()
    {
        var properties = typeof(T).GetProperties()
            .Where(p => p.Name != "Id" && p.Name != "CreatedOn" && p.Name != "CreatedBy" && p.Name != "SocietyId")
            .Select(p => $"{p.Name} = @{p.Name}");

        var setClause = string.Join(", ", properties);

        return $@"
                    UPDATE {_tableName}
                    SET {setClause}
                    OUTPUT INSERTED.*
                    WHERE Id = @Id
                    ";
    }
}
