using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Contact.Application.Abstractions.Messaging;

public static class MessagingServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="ISender"/> and every concrete <see cref="IRequestHandler{TRequest, TResponse}"/>
    /// in <paramref name="assembly"/> as scoped services.
    /// </summary>
    /// <remarks>
    /// Assembly scanning instead of one AddScoped line per handler: with one handler per controller
    /// action the explicit list would grow to dozens of lines that are easy to forget.
    /// Contact.Application.Tests guards the scan by asserting that every request type has exactly one handler.
    /// </remarks>
    public static IServiceCollection AddRequestHandlers(this IServiceCollection services, Assembly assembly)
    {
        services.AddScoped<ISender, Sender>();

        var registrations =
            from type in assembly.GetTypes()
            where type is { IsClass: true, IsAbstract: false, IsGenericTypeDefinition: false }
            from contract in type.GetInterfaces()
            where contract.IsGenericType && contract.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)
            select (contract, type);

        foreach (var (contract, implementation) in registrations)
        {
            services.AddScoped(contract, implementation);
        }

        return services;
    }
}
