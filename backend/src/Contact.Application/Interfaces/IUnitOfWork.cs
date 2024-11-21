using System.Data;

namespace Contact.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IDbTransaction BeginTransaction();
    Task CommitAsync();
    Task RollbackAsync();
}
