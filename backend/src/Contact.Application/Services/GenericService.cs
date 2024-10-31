using AutoMapper;
using Contact.Application.Interfaces;
using Contact.Domain.Entities;
using Contact.Domain.Interfaces;

namespace Contact.Application.Services;

public class GenericService<TEntity, TResponse, TCreate, TUpdate>
        : IGenericService<TEntity, TResponse, TCreate, TUpdate>
        where TEntity : BaseEntity
        where TResponse : class
        where TCreate : class
        where TUpdate : class
{
    private readonly IGenericRepository<TEntity> _repository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GenericService(IGenericRepository<TEntity> repository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Add(TCreate createDto)
    {
        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            var entity = _mapper.Map<TEntity>(createDto);
            var createdEntity = await _repository.Add(entity, transaction);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<TResponse>(createdEntity); // Map to clean response DTO
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<TResponse> Update(TUpdate updateDto)
    {
        var entity = _mapper.Map<TEntity>(updateDto);
        var updatedEntity = await _repository.Update(entity);
        return _mapper.Map<TResponse>(updatedEntity); // Map to clean response DTO
    }

    public async Task<bool> Delete(Guid id)
    {
        return await _repository.Delete(id);
    }

    public async Task<TResponse> FindByID(Guid id)
    {
        var entity = await _repository.FindByID(id);
        return _mapper.Map<TResponse>(entity); // Map to clean response DTO
    }

    public async Task<IEnumerable<TResponse>> FindAll()
    {
        var entities = await _repository.FindAll();
        return _mapper.Map<IEnumerable<TResponse>>(entities); // Map to clean response DTOs
    }
}
