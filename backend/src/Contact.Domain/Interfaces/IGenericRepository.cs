using Contact.Domain.Entities;
using System.Data;

namespace Contact.Domain.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T> Add(T item, IDbTransaction? transaction = null);
    Task<bool> Delete(Guid id, IDbTransaction? transaction = null);
    Task<T> Update(T item, IDbTransaction? transaction = null);
    Task<T> FindByID(Guid id);
    Task<IEnumerable<T>> Find(string query, object? parameters = null);
    Task<IEnumerable<T>> FindAll();
}
