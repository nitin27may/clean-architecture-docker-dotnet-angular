namespace Contact.Application.Abstractions.Messaging;

/// <summary>
/// Marks a command or query that produces a <typeparamref name="TResponse"/>.
/// Each request type has exactly one <see cref="IRequestHandler{TRequest, TResponse}"/>.
/// </summary>
/// <remarks>See docs/adr/0002-mediator-abstraction.md for why this is in-repo rather than MediatR.</remarks>
public interface IRequest<TResponse>;
