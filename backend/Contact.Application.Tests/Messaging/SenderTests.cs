using Contact.Application.Abstractions.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Contact.Application.Tests.Messaging;

public class SenderTests
{
    public sealed record Ping(string Message) : IRequest<string>;

    public sealed class PingHandler : IRequestHandler<Ping, string>
    {
        public Task<string> Handle(Ping request, CancellationToken cancellationToken)
            => Task.FromResult($"pong: {request.Message}");
    }

    public sealed record Unhandled : IRequest<int>;

    public sealed record WaitForCancellation : IRequest<bool>;

    public sealed class WaitForCancellationHandler : IRequestHandler<WaitForCancellation, bool>
    {
        public Task<bool> Handle(WaitForCancellation request, CancellationToken cancellationToken)
            => Task.FromResult(cancellationToken.IsCancellationRequested);
    }

    private static ISender BuildSender()
    {
        var services = new ServiceCollection();
        services.AddRequestHandlers(typeof(SenderTests).Assembly);
        return services.BuildServiceProvider().CreateScope().ServiceProvider.GetRequiredService<ISender>();
    }

    [Fact]
    public async Task Send_dispatches_to_the_registered_handler()
    {
        var response = await BuildSender().Send(new Ping("hello"), TestContext.Current.CancellationToken);

        Assert.Equal("pong: hello", response);
    }

    [Fact]
    public async Task Send_is_repeatable_for_the_same_request_type()
    {
        var sender = BuildSender();

        Assert.Equal("pong: a", await sender.Send(new Ping("a"), TestContext.Current.CancellationToken));
        Assert.Equal("pong: b", await sender.Send(new Ping("b"), TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task Send_passes_the_cancellation_token_to_the_handler()
    {
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        Assert.True(await BuildSender().Send(new WaitForCancellation(), cts.Token));
    }

    [Fact]
    public async Task Send_without_a_handler_names_the_missing_request_type()
    {
        var error = await Assert.ThrowsAsync<InvalidOperationException>(
            () => BuildSender().Send(new Unhandled(), TestContext.Current.CancellationToken));

        Assert.Contains(nameof(Unhandled), error.Message);
    }

    [Fact]
    public async Task Send_rejects_a_null_request()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => BuildSender().Send<string>(null!, TestContext.Current.CancellationToken));
    }
}
