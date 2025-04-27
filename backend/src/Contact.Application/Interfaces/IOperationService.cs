using Contact.Application.UseCases.Operations;
using Contact.Domain.Entities;

namespace Contact.Application.Interfaces;

public interface IOperationService : IGenericService<Operation, OperationResponse, CreateOperation, UpdateOperation>
{
}
