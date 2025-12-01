using AutoMapper;
using Contact.Application.Interfaces;
using Contact.Application.UseCases.Operations;
using Contact.Domain.Entities;
using Contact.Domain.Interfaces;

namespace Contact.Application.Services;

public class OperationService : GenericService<Operation, OperationResponse, CreateOperation, UpdateOperation>, IOperationService
{
    public OperationService(IGenericRepository<Operation> repository, IMapper mapper, IUnitOfWork unitOfWork)
        : base(repository, mapper, unitOfWork)
    {
    }
}
