using Contact.Application.Abstractions.Messaging;
using Contact.Application.Interfaces;
using Contact.Application.Tests.UseCases;
using Contact.Application.UseCases.ContactPerson;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Contact.Application.Tests.Messaging;

public class ValidationTests
{
    public sealed record Greet(string Name) : IRequest<string>;

    public sealed class GreetValidator : AbstractValidator<Greet>
    {
        public GreetValidator() => RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
    }

    public sealed class GreetHandler : IRequestHandler<Greet, string>
    {
        public static int Calls;

        public Task<string> Handle(Greet request, CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref Calls);
            return Task.FromResult($"hello {request.Name}");
        }
    }

    private static ISender BuildSender()
    {
        var services = new ServiceCollection();
        services.AddRequestHandlers(typeof(ValidationTests).Assembly);
        services.AddValidatorsFromAssemblyContaining<ValidationTests>();
        return services.BuildServiceProvider().CreateScope().ServiceProvider.GetRequiredService<ISender>();
    }

    [Fact]
    public async Task Valid_request_reaches_the_handler()
    {
        Assert.Equal("hello Ada", await BuildSender().Send(new Greet("Ada"), TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task Invalid_request_throws_with_every_failure_and_skips_the_handler()
    {
        var callsBefore = Volatile.Read(ref GreetHandler.Calls);

        var error = await Assert.ThrowsAsync<ValidationException>(
            () => BuildSender().Send(new Greet(""), TestContext.Current.CancellationToken));

        var failure = Assert.Single(error.Errors);
        Assert.Equal(nameof(Greet.Name), failure.PropertyName);
        Assert.Equal("Name is required", failure.ErrorMessage);
        Assert.Equal(callsBefore, Volatile.Read(ref GreetHandler.Calls));
    }

    [Fact]
    public async Task Create_contact_rules_run_before_the_service_is_called()
    {
        // Uses the real Contact.Application registrations, so this exercises CreateContactPersonValidator,
        // which was registered but never invoked before the mediator existed.
        var services = new ServiceCollection();
        services.AddRequestHandlers(typeof(ApplicationServiceCollectionExtensions).Assembly);
        services.AddValidatorsFromAssemblyContaining<CreateContactPersonValidator>();
        services.AddScoped<IContactPersonService, ThrowingContactPersonService>();
        var sender = services.BuildServiceProvider().CreateScope().ServiceProvider.GetRequiredService<ISender>();

        var request = CreateContactPersonHandlerTests.ValidRequest();
        request.Email = "not-an-email";
        request.FirstName = "";

        var error = await Assert.ThrowsAsync<ValidationException>(
            () => sender.Send(request, TestContext.Current.CancellationToken));

        Assert.Equal(
            [nameof(CreateContactPerson.Email), nameof(CreateContactPerson.FirstName)],
            error.Errors.Select(e => e.PropertyName).Distinct().Order().ToArray());
    }

    [Fact]
    public async Task Valid_create_contact_request_reaches_the_service()
    {
        var services = new ServiceCollection();
        services.AddRequestHandlers(typeof(ApplicationServiceCollectionExtensions).Assembly);
        services.AddValidatorsFromAssemblyContaining<CreateContactPersonValidator>();
        services.AddScoped<IContactPersonService, ThrowingContactPersonService>();
        var sender = services.BuildServiceProvider().CreateScope().ServiceProvider.GetRequiredService<ISender>();

        // Passing validation means the handler runs and hits the (throwing) service.
        await Assert.ThrowsAsync<ServiceReachedException>(
            () => sender.Send(CreateContactPersonHandlerTests.ValidRequest(), TestContext.Current.CancellationToken));
    }

    private sealed class ServiceReachedException() : Exception("The contact service was called.");

    private sealed class ThrowingContactPersonService : IContactPersonService
    {
        public Task<ContactPersonResponse> Add(CreateContactPerson createDto) => throw new ServiceReachedException();
        public Task<ContactPersonResponse> Update(UpdateContactPerson updateDto) => throw new ServiceReachedException();
        public Task<bool> Delete(Guid id) => throw new ServiceReachedException();
        public Task<ContactPersonResponse> FindByID(Guid id) => throw new ServiceReachedException();
        public Task<IEnumerable<ContactPersonResponse>> FindAll() => throw new ServiceReachedException();
    }
}
