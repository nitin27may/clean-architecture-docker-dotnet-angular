using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;

namespace Contact.Application.Abstractions.Messaging;

/// <summary>
/// Resolves the handler for a request from the DI container.
/// </summary>
/// <remarks>
/// <see cref="ISender.Send{TResponse}"/> only knows the response type statically, so the request's
/// runtime type is bridged once per request type into a closed generic dispatcher and cached.
/// After the first call for a given request type, dispatch is a dictionary lookup plus a
/// virtual call; no per-call reflection.
/// </remarks>
internal sealed class Sender(IServiceProvider serviceProvider) : ISender
{
    private static readonly ConcurrentDictionary<Type, object> Dispatchers = new();

    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var dispatcher = (RequestDispatcher<TResponse>)Dispatchers.GetOrAdd(
            request.GetType(),
            static (requestType, responseType) => Activator.CreateInstance(
                typeof(RequestDispatcher<,>).MakeGenericType(requestType, responseType))!,
            typeof(TResponse));

        return dispatcher.Dispatch(request, serviceProvider, cancellationToken);
    }

    private abstract class RequestDispatcher<TResponse>
    {
        public abstract Task<TResponse> Dispatch(
            IRequest<TResponse> request, IServiceProvider serviceProvider, CancellationToken cancellationToken);
    }

    private sealed class RequestDispatcher<TRequest, TResponse> : RequestDispatcher<TResponse>
        where TRequest : IRequest<TResponse>
    {
        public override Task<TResponse> Dispatch(
            IRequest<TResponse> request, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var handler = serviceProvider.GetService<IRequestHandler<TRequest, TResponse>>()
                ?? throw new InvalidOperationException(
                    $"No handler is registered for '{typeof(TRequest).FullName}'. " +
                    $"Add a class implementing IRequestHandler<{typeof(TRequest).Name}, {typeof(TResponse).Name}> " +
                    "to an assembly passed to AddRequestHandlers.");

            return handler.Handle((TRequest)request, cancellationToken);
        }
    }
}
