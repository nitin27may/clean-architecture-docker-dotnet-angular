namespace Contact.Application.Abstractions.Messaging;

/// <summary>Handles one request type. Registered automatically by <c>AddRequestHandlers</c>.</summary>
public interface IRequestHandler<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}
