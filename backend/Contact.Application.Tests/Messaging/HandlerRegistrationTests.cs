using Contact.Application.Abstractions.Messaging;

namespace Contact.Application.Tests.Messaging;

/// <summary>
/// Guards the assembly scan in AddRequestHandlers: a request without a handler only fails when
/// someone calls the endpoint, so it is caught here at build time instead.
/// </summary>
public class HandlerRegistrationTests
{
    private static readonly Type[] ApplicationTypes = typeof(ApplicationServiceCollectionExtensions).Assembly.GetTypes();

    public static TheoryData<Type> RequestTypes()
    {
        var data = new TheoryData<Type>();
        foreach (var type in ApplicationTypes.Where(t => t is { IsAbstract: false, IsInterface: false } && RequestContract(t) is not null))
        {
            data.Add(type);
        }
        return data;
    }

    [Theory]
    [MemberData(nameof(RequestTypes))]
    public void Every_request_has_exactly_one_handler(Type requestType)
    {
        var responseType = RequestContract(requestType)!.GetGenericArguments()[0];
        var handlerContract = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);

        var handlers = ApplicationTypes
            .Where(t => t is { IsClass: true, IsAbstract: false } && handlerContract.IsAssignableFrom(t))
            .ToList();

        Assert.Single(handlers);
    }

    [Fact]
    public void Application_assembly_contains_at_least_one_request()
    {
        // Without this, an empty theory would pass vacuously if the scan found nothing.
        Assert.NotEmpty(RequestTypes());
    }

    private static Type? RequestContract(Type type) => type.GetInterfaces()
        .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequest<>));
}
