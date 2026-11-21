namespace Contact.Application.Abstractions.Messaging;

/// <summary>What controllers depend on instead of application services.</summary>
public interface ISender
{
    /// <summary>Dispatches <paramref name="request"/> to its single registered handler.</summary>
    /// <exception cref="InvalidOperationException">No handler is registered for the request type.</exception>
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}
