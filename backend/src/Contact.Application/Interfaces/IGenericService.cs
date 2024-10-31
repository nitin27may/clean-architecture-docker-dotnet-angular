namespace Contact.Application.Interfaces;

public interface IGenericService<TEntity, TResponse, TCreate, TUpdate>
       where TEntity : class
       where TResponse : class
       where TCreate : class
       where TUpdate : class
{
    Task<TResponse> Add(TCreate createDto);
    Task<TResponse> Update(TUpdate updateDto);
    Task<bool> Delete(Guid id);
    Task<TResponse> FindByID(Guid id);
    Task<IEnumerable<TResponse>> FindAll();
}
