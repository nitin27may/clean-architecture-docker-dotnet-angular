using System.Security.Claims;
using System.Text.Json;
using Contact.Application.Abstractions.Messaging;
using Contact.Application.Interfaces;
using Contact.Application.Mapping;
using Contact.Application.UseCases.Users;
using Contact.Domain.Entities;
using Contact.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Contact.Application.Tests.UseCases;

/// <summary>
/// User requests sent through the real <see cref="ISender"/>, handler and validator registrations and
/// the real <see cref="ObjectMapper"/>, against a hand-written <see cref="IUserService"/> fake.
/// </summary>
public class UserHandlerTests
{
    private readonly FakeUserService users = new();
    private readonly ISender sender;

    public UserHandlerTests()
    {
        var services = new ServiceCollection();
        services.AddRequestHandlers(typeof(ApplicationServiceCollectionExtensions).Assembly);
        services.AddValidatorsFromAssemblyContaining<RegisterUserValidator>();
        services.AddSingleton<IUserService>(users);
        services.AddSingleton<IHttpContextAccessor>(new HttpContextAccessor());
        services.AddScoped<IMapper, ObjectMapper>();
        sender = services.BuildServiceProvider().CreateScope().ServiceProvider.GetRequiredService<ISender>();
    }

    private static CancellationToken Ct => TestContext.Current.CancellationToken;

    [Fact]
    public async Task Register_returns_the_user_without_the_password_hash()
    {
        var response = await sender.Send(ValidRegistration(), Ct);

        Assert.Equal("ada@example.com", response.Email);
        var json = JsonSerializer.Serialize<object>(response);
        Assert.DoesNotContain("password", json, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain(FakeUserService.PasswordHash, json);
    }

    [Fact]
    public async Task Register_rejects_an_existing_user()
    {
        users.ExistingUsers.Add(users.Entity("ada@example.com"));

        var error = await Assert.ThrowsAsync<BusinessException>(() => sender.Send(ValidRegistration(), Ct));

        Assert.Equal("User already exists", error.Message);
        Assert.Equal(0, users.AddCalls);
    }

    [Fact]
    public async Task Register_accepts_an_email_address_as_the_username()
    {
        var request = ValidRegistration();
        request.Username = "ada.lovelace+test@example.com";

        Assert.Equal("ada@example.com", (await sender.Send(request, Ct)).Email);
    }

    [Fact]
    public async Task Register_enforces_the_password_rules()
    {
        var request = ValidRegistration();
        request.Password = "password";

        var error = await Assert.ThrowsAsync<ValidationException>(() => sender.Send(request, Ct));

        Assert.All(error.Errors, e => Assert.Equal(nameof(RegisterUser.Password), e.PropertyName));
        Assert.Equal(0, users.AddCalls);
    }

    [Fact]
    public async Task Create_checks_uniqueness_by_email_and_username()
    {
        var request = new CreateUser
        {
            FirstName = "Grace", LastName = "Hopper", Mobile = "+15550100", Role = "Reader",
            UserName = "grace", Email = "grace@example.com",
        };

        var response = await sender.Send(request, Ct);

        Assert.Equal(("grace@example.com", "grace"), users.LastUniquenessCheck);
        Assert.Equal("grace@example.com", response.Email);
    }

    [Fact]
    public async Task Create_requires_a_role()
    {
        var request = new CreateUser
        {
            FirstName = "Grace", LastName = "Hopper", Mobile = "+15550100", Role = "",
            UserName = "grace", Email = "grace@example.com",
        };

        var error = await Assert.ThrowsAsync<ValidationException>(() => sender.Send(request, Ct));

        Assert.Contains(error.Errors, e => e.PropertyName == nameof(CreateUser.Role));
    }

    [Fact]
    public async Task Update_returns_null_for_an_unknown_user()
    {
        Assert.Null(await sender.Send(ValidUpdate(Guid.NewGuid()), Ct));
        Assert.Equal(0, users.UpdateCalls);
    }

    [Fact]
    public async Task Update_rejects_an_invalid_email_before_touching_the_user()
    {
        var request = ValidUpdate(users.Seed().Id);
        request.Email = "not-an-email";

        await Assert.ThrowsAsync<ValidationException>(() => sender.Send(request, Ct));
        Assert.Equal(0, users.UpdateCalls);
    }

    [Fact]
    public async Task Update_saves_a_valid_profile()
    {
        var existing = users.Seed();

        var updated = await sender.Send(ValidUpdate(existing.Id), Ct);

        Assert.NotNull(updated);
        Assert.Equal(1, users.UpdateCalls);
    }

    private static RegisterUser ValidRegistration() => new()
    {
        FirstName = "Ada",
        LastName = "Lovelace",
        Username = "ada.lovelace",
        Mobile = "+447946000000",
        Email = "ada@example.com",
        Password = "Str0ng!Passw0rd",
    };

    private static UpdateUser ValidUpdate(Guid id) => new()
    {
        Id = id,
        FirstName = "Ada",
        LastName = "Lovelace",
        UserName = "ada@example.com",
        Mobile = "+44 20 7946 0000",
        Email = "ada@example.com",
    };

    /// <summary>Hand-written fake: the repo avoids mocking libraries with non-permissive licences (ADR 0001).</summary>
    private sealed class FakeUserService : IUserService
    {
        public const string PasswordHash = "AQAAAAIAAYagAAAAE-fake-hash";

        public List<User> ExistingUsers { get; } = [];
        public Dictionary<Guid, UserResponse> Profiles { get; } = [];
        public (string Email, string Username)? LastUniquenessCheck { get; private set; }
        public int AddCalls { get; private set; }
        public int UpdateCalls { get; private set; }

        public User Entity(string email) => new()
        {
            Id = Guid.NewGuid(), FirstName = "Ada", LastName = "Lovelace", UserName = email,
            Mobile = "+447946000000", Email = email, Password = PasswordHash,
            CreatedOn = DateTimeOffset.UtcNow, CreatedBy = Guid.Empty,
        };

        public UserResponse Seed()
        {
            var profile = new UserResponse { Id = Guid.NewGuid(), Email = "ada@example.com" };
            Profiles[profile.Id] = profile;
            return profile;
        }

        public Task<IEnumerable<User>> CheckUniqueUsers(string email, string username)
        {
            LastUniquenessCheck = (email, username);
            return Task.FromResult<IEnumerable<User>>(
                ExistingUsers.Where(u => u.Email == email || u.UserName == username).ToList());
        }

        public Task<User> Add(RegisterUser user)
        {
            AddCalls++;
            return Task.FromResult(Entity(user.Email));
        }

        public Task<User> Create(CreateUser createUser) => Task.FromResult(Entity(createUser.Email));

        public Task<UserResponse> FindByID(Guid id) => Task.FromResult(Profiles.GetValueOrDefault(id)!);

        public Task<UserResponse> Update(UpdateUser user)
        {
            UpdateCalls++;
            return Task.FromResult(new UserResponse { Id = user.Id, Email = user.Email });
        }

        public Task<bool> Delete(Guid id) => throw new NotSupportedException();
        public Task<IEnumerable<UserResponse>> FindAll() => throw new NotSupportedException();
        public Task<AuthenticateResponse> Authenticate(AuthenticateRequest authenticateRequest) => throw new NotSupportedException();
        public Guid GetUserId(ClaimsPrincipal user) => throw new NotSupportedException();
        public Task<IEnumerable<string>> GetUserRolesAsync(ClaimsPrincipal user) => throw new NotSupportedException();
        public Task<bool> ChangePassword(ChangePassword changePassword) => throw new NotSupportedException();
        public Task<bool> ForgotPassword(string email) => throw new NotSupportedException();
        public Task<bool> ResetPassword(ResetPassword resetPasswordRequest) => throw new NotSupportedException();
        public Task<UserResponse> GetUserWithPermissionsAsync(Guid userId) => throw new NotSupportedException();
        public Task<UserResponse> UpdateUserRoles(Guid userId, UpdateUserRoles updateUserRoles) => throw new NotSupportedException();
        public Task<UserWithRolesResponse> GetUserWithRolesAsync(Guid userId) => throw new NotSupportedException();
    }
}
