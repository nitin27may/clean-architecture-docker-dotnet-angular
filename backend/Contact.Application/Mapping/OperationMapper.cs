using Contact.Domain.Entities;
using Riok.Mapperly.Abstractions;
using OperationResponse = Contact.Application.UseCases.Operations.OperationResponse;
using CreateOperation = Contact.Application.UseCases.Operations.CreateOperation;
using UpdateOperation = Contact.Application.UseCases.Operations.UpdateOperation;

namespace Contact.Application.Mapping;

[Mapper]
public partial class OperationMapper
{
    public partial Operation ToOperation(CreateOperation source, DateTimeOffset createdOn, Guid createdBy);

    // See UserMapper.ToUser(UpdateUser, ...) for why the audit placeholders are safe here.
    public partial Operation ToOperation(UpdateOperation source, DateTimeOffset createdOn, Guid createdBy);

    public partial OperationResponse ToOperationResponse(Operation source);
}
