using Contact.Application.Interfaces;
using Contact.Infrastructure.Persistence.Helper;
using System.Data;

namespace Contact.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly IDapperHelper _dapperHelper;
    private IDbTransaction _transaction;
    private IDbConnection _connection;

    public UnitOfWork(IDapperHelper dapperHelper)
    {
        _dapperHelper = dapperHelper;
        _connection = _dapperHelper.GetConnection();
    }

    public IDbTransaction BeginTransaction()
    {
        if (_connection.State == ConnectionState.Closed)
            _connection.Open();

        _transaction = _connection.BeginTransaction();
        return _transaction;
    }

    public async Task CommitAsync()
    {
        try
        {
            _transaction?.Commit();
            await Task.CompletedTask;
        }
        finally
        {
            Dispose();
        }
    }

    public async Task RollbackAsync()
    {
        try
        {
            _transaction?.Rollback();
            await Task.CompletedTask;
        }
        finally
        {
            Dispose();
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        if (_connection?.State == ConnectionState.Open)
        {
            _connection.Close();
        }
        _connection?.Dispose();
    }
}
