using Contact.Application.Abstractions.Messaging;
using Contact.Application.Interfaces;
using Contact.Application.UseCases.ContactPerson;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Contact.Application.Tests.UseCases;

/// <summary>
/// The read, update and delete contact handlers, sent through the real <see cref="ISender"/> and
/// Contact.Application registrations (handlers + validators), against an in-memory service.
/// </summary>
public class ContactPersonHandlerTests
{
    private readonly InMemoryContactPersonService service = new();
    private readonly ISender sender;

    public ContactPersonHandlerTests()
    {
        var services = new ServiceCollection();
        services.AddRequestHandlers(typeof(ApplicationServiceCollectionExtensions).Assembly);
        services.AddValidatorsFromAssemblyContaining<CreateContactPersonValidator>();
        services.AddSingleton<IContactPersonService>(service);
        sender = services.BuildServiceProvider().CreateScope().ServiceProvider.GetRequiredService<ISender>();
    }

    private static CancellationToken Ct => TestContext.Current.CancellationToken;

    [Fact]
    public async Task GetById_returns_the_contact()
    {
        var existing = service.Seed("ada@example.com");

        var found = await sender.Send(new GetContactPersonById(existing.Id), Ct);

        Assert.NotNull(found);
        Assert.Equal("ada@example.com", found.Email);
    }

    [Fact]
    public async Task GetById_returns_null_for_an_unknown_id()
    {
        Assert.Null(await sender.Send(new GetContactPersonById(Guid.NewGuid()), Ct));
    }

    [Fact]
    public async Task GetAll_returns_every_contact()
    {
        service.Seed("a@example.com");
        service.Seed("b@example.com");

        var all = await sender.Send(new GetAllContactPersons(), Ct);

        Assert.Equal(["a@example.com", "b@example.com"], all.Select(c => c.Email).Order().ToArray());
    }

    [Fact]
    public async Task Update_changes_the_contact()
    {
        var existing = service.Seed("old@example.com");

        var updated = await sender.Send(UpdateRequest(existing.Id, "new@example.com"), Ct);

        Assert.NotNull(updated);
        Assert.Equal("new@example.com", updated.Email);
        Assert.Equal("new@example.com", service.Contacts[existing.Id].Email);
    }

    [Fact]
    public async Task Update_returns_null_and_writes_nothing_for_an_unknown_id()
    {
        var updated = await sender.Send(UpdateRequest(Guid.NewGuid(), "new@example.com"), Ct);

        Assert.Null(updated);
        Assert.Equal(0, service.UpdateCalls);
    }

    [Fact]
    public async Task Update_with_invalid_fields_is_rejected_before_the_service_is_called()
    {
        var existing = service.Seed("old@example.com");

        var error = await Assert.ThrowsAsync<ValidationException>(
            () => sender.Send(UpdateRequest(existing.Id, "not-an-email"), Ct));

        Assert.Contains(error.Errors, e => e.PropertyName == nameof(UpdateContactPerson.Email));
        Assert.Equal(0, service.FindCalls);
        Assert.Equal(0, service.UpdateCalls);
    }

    [Fact]
    public async Task Delete_removes_the_contact()
    {
        var existing = service.Seed("ada@example.com");

        Assert.True(await sender.Send(new DeleteContactPerson(existing.Id), Ct));
        Assert.Empty(service.Contacts);
    }

    [Fact]
    public async Task Delete_returns_false_for_an_unknown_id()
    {
        Assert.False(await sender.Send(new DeleteContactPerson(Guid.NewGuid()), Ct));
    }

    private static UpdateContactPerson UpdateRequest(Guid id, string email) => new()
    {
        Id = id,
        FirstName = "Ada",
        LastName = "Lovelace",
        DateOfBirth = new DateOnly(1990, 12, 10),
        Mobile = "+44 20 7946 0000",
        Email = email,
        City = "London",
        PostalCode = "N1 9GU",
    };

    /// <summary>Hand-written fake: the repo avoids mocking libraries with non-permissive licences (ADR 0001).</summary>
    private sealed class InMemoryContactPersonService : IContactPersonService
    {
        public Dictionary<Guid, ContactPersonResponse> Contacts { get; } = [];
        public int FindCalls { get; private set; }
        public int UpdateCalls { get; private set; }

        public ContactPersonResponse Seed(string email)
        {
            var contact = Response(Guid.NewGuid(), email);
            Contacts[contact.Id] = contact;
            return contact;
        }

        public Task<ContactPersonResponse> FindByID(Guid id)
        {
            FindCalls++;
            // Mirrors the real service, whose mapper turns a missing row into null.
            return Task.FromResult(Contacts.GetValueOrDefault(id)!);
        }

        public Task<IEnumerable<ContactPersonResponse>> FindAll()
            => Task.FromResult<IEnumerable<ContactPersonResponse>>(Contacts.Values.ToList());

        public Task<ContactPersonResponse> Update(UpdateContactPerson updateDto)
        {
            UpdateCalls++;
            var contact = Response(updateDto.Id, updateDto.Email);
            Contacts[contact.Id] = contact;
            return Task.FromResult(contact);
        }

        public Task<bool> Delete(Guid id) => Task.FromResult(Contacts.Remove(id));

        public Task<ContactPersonResponse> Add(CreateContactPerson createDto) => throw new NotSupportedException();

        private static ContactPersonResponse Response(Guid id, string email) => new()
        {
            Id = id,
            FirstName = "Ada",
            LastName = "Lovelace",
            DateOfBirth = new DateOnly(1990, 12, 10),
            Mobile = "+44 20 7946 0000",
            Email = email,
            City = "London",
            PostalCode = "N1 9GU",
        };
    }
}
