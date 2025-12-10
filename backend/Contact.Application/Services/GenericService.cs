using AutoMapper;
using Contact.Application.Interfaces;
using Contact.Domain.Entities;
using Contact.Domain.Interfaces;

namespace Contact.Application.Services;

public class GenericService<TEntity, TResponse, TCreate, TUpdate>(
    IGenericRepository<TEntity> repository,
    IMapper mapper,
    IUnitOfWork unitOfWork)
        : IGenericService<TEntity, TResponse, TCreate, TUpdate>
        where TEntity : BaseEntity
        where TResponse : class
        where TCreate : class
        where TUpdate : class
{
    public async Task<TResponse> Add(TCreate createDto)
    {
        using var transaction = unitOfWork.BeginTransaction();
        try
        {
            var entity = mapper.Map<TEntity>(createDto);
            var createdEntity = await repository.Add(entity, transaction);
            await unitOfWork.CommitAsync();
            return mapper.Map<TResponse>(createdEntity);
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<TResponse> Update(TUpdate updateDto)
    {
        using var transaction = unitOfWork.BeginTransaction();
        try
        {
            var entity = mapper.Map<TEntity>(updateDto);
            var updatedEntity = await repository.Update(entity);
            await unitOfWork.CommitAsync();
            return mapper.Map<TResponse>(updatedEntity);
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> Delete(Guid id)
    {
        using var transaction = unitOfWork.BeginTransaction();
        try
        {
            var result = await repository.Delete(id);
            await unitOfWork.CommitAsync();
            return result;
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<TResponse> FindByID(Guid id)
    {
        var entity = await repository.FindByID(id);
        return mapper.Map<TResponse>(entity);
    }

    public async Task<IEnumerable<TResponse>> FindAll()
    {
        var entities = await repository.FindAll();
        return mapper.Map<IEnumerable<TResponse>>(entities);
    }
}
