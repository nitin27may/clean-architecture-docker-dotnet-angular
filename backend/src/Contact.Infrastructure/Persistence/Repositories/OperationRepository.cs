using Contact.Domain.Entities;
using Contact.Domain.Interfaces;
using Contact.Infrastructure.Persistence.Helper;
using Dapper;
using System.Data;

namespace Contact.Infrastructure.Persistence.Repositories;

public class OperationRepository(IDapperHelper dapperHelper) 
    : GenericRepository<Operation>(dapperHelper, "Operations"), 
      IOperationRepository
{
    public async Task<Operation> GetOperationById(Guid id, IDbTransaction? transaction = null)
    {
        var parameters = new DynamicParameters();
        parameters.Add("Id", id);
        return await dapperHelper.Get<Operation>(@"SELECT * FROM ""Operations"" WHERE ""Id"" = @Id", parameters, CommandType.Text, transaction);
    }

    public async Task<Operation> AddOperation(Operation operation, IDbTransaction? transaction = null)
    {
        var dbPara = new DynamicParameters();
        dbPara.Add("Name", operation.Name);
        dbPara.Add("Description", operation.Description);
        dbPara.Add("CreatedBy", operation.CreatedBy);
        dbPara.Add("CreatedOn", operation.CreatedOn);

        return await dapperHelper.Insert<Operation>(@"
            INSERT INTO ""Operations"" (""Name"", ""Description"", ""CreatedBy"", ""CreatedOn"")
            VALUES (@Name, @Description, @CreatedBy, @CreatedOn)
            RETURNING *",
            dbPara, CommandType.Text, transaction);
    }

    public async Task<Operation> UpdateOperation(Operation operation, IDbTransaction? transaction = null)
    {
        var dbPara = new DynamicParameters();
        dbPara.Add("Id", operation.Id);
        dbPara.Add("Name", operation.Name);
        dbPara.Add("Description", operation.Description);
        dbPara.Add("UpdatedBy", operation.UpdatedBy);
        dbPara.Add("UpdatedOn", operation.UpdatedOn);

        return await dapperHelper.Update<Operation>(@"
            UPDATE ""Operations"" 
            SET 
                ""Name"" = @Name,  
                ""Description"" = @Description, 
                ""UpdatedBy"" = @UpdatedBy,
                ""UpdatedOn"" = @UpdatedOn
            WHERE ""Id"" = @Id
            RETURNING *",
            dbPara, CommandType.Text, transaction);
    }

    public async Task<bool> DeleteOperation(Guid id, IDbTransaction? transaction = null)
    {
        var dbPara = new DynamicParameters();
        dbPara.Add("Id", id);

        var result = await dapperHelper.Execute(@"
            DELETE FROM ""Operations"" 
            WHERE ""Id"" = @Id",
            dbPara, CommandType.Text, transaction);

        return result > 0;
    }
}
