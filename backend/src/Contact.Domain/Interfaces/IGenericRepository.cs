using System.Data;
using Contact.Domain.Entities;

namespace Contact.Domain.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T> Add(T entity, IDbTransaction? transaction = null);
    Task<T> Update(T entity, IDbTransaction? transaction = null);
    Task<bool> Delete(Guid id, IDbTransaction? transaction = null);
    Task<T?> FindByID(Guid id, IDbTransaction? transaction = null);
    Task<IEnumerable<T>> FindAll(IDbTransaction? transaction = null);
}
