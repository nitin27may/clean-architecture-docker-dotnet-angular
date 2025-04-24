using Contact.Application.Interfaces;
using Contact.Infrastructure.Persistence.Helper;
using System.Data;

namespace Contact.Infrastructure.Persistence;

public class UnitOfWork(IDapperHelper dapperHelper) : IUnitOfWork
{
    private IDbTransaction? _transaction;
    private IDbConnection? _connection;
    private bool _disposed;

    public IDbTransaction BeginTransaction()
    {
        _connection = dapperHelper.GetConnection();
        
        // Ensure the connection is open before starting a transaction
        if (_connection.State != ConnectionState.Open)
        {
            _connection.Open();
        }
        
        _transaction = _connection.BeginTransaction();
        return _transaction;
    }

    public async Task CommitAsync()
    {
        try
        {
            _transaction?.Commit();
        }
        catch
        {
            _transaction?.Rollback();
            throw;
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
            
            // Close the connection when transaction is committed
            if (_connection?.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        await Task.CompletedTask;
    }

    public async Task RollbackAsync()
    {
        try
        {
            _transaction?.Rollback();
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
            
            // Close the connection when transaction is rolled back
            if (_connection?.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        await Task.CompletedTask;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _transaction?.Dispose();
                _connection?.Dispose();
            }
            _disposed = true;
        }
    }
}
