using System.Collections.Concurrent;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Contact.Application.Abstractions.Messaging;

/// <summary>
/// Validates a request with every registered FluentValidation validator for its type, then
/// resolves and invokes its handler from the DI container.
/// </summary>
/// <remarks>
/// <see cref="ISender.Send{TResponse}"/> only knows the response type statically, so the request's
/// runtime type is bridged once per request type into a closed generic dispatcher and cached.
/// After the first call for a given request type, dispatch is a dictionary lookup plus a
/// virtual call; no per-call reflection.
/// <para>
/// Validation runs before the handler is resolved. Any failure throws
/// <see cref="ValidationException"/> carrying every failure, and the handler is not called;
/// ExceptionMiddleware turns that into a 400 response.
/// </para>
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
        public override async Task<TResponse> Dispatch(
            IRequest<TResponse> request, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var typedRequest = (TRequest)request;

            await ValidateAsync(typedRequest, serviceProvider, cancellationToken);

            var handler = serviceProvider.GetService<IRequestHandler<TRequest, TResponse>>()
                ?? throw new InvalidOperationException(
                    $"No handler is registered for '{typeof(TRequest).FullName}'. " +
                    $"Add a class implementing IRequestHandler<{typeof(TRequest).Name}, {typeof(TResponse).Name}> " +
                    "to an assembly passed to AddRequestHandlers.");

            return await handler.Handle(typedRequest, cancellationToken);
        }

        private static async Task ValidateAsync(
            TRequest request, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var validators = serviceProvider.GetServices<IValidator<TRequest>>().ToArray();
            if (validators.Length == 0)
            {
                return;
            }

            var context = new ValidationContext<TRequest>(request);
            var failures = new List<FluentValidation.Results.ValidationFailure>();
            foreach (var validator in validators)
            {
                var result = await validator.ValidateAsync(context, cancellationToken);
                failures.AddRange(result.Errors);
            }

            if (failures.Count > 0)
            {
                throw new ValidationException(failures);
            }
        }
    }
}
