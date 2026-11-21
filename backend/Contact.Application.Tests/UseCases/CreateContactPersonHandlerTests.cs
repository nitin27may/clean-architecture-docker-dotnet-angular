using Contact.Application.Interfaces;
using Contact.Application.UseCases.ContactPerson;

namespace Contact.Application.Tests.UseCases;

public class CreateContactPersonHandlerTests
{
    [Fact]
    public async Task Handle_creates_the_contact_through_the_existing_service()
    {
        var service = new RecordingContactPersonService();
        var request = ValidRequest();

        var response = await new CreateContactPersonHandler(service).Handle(request, TestContext.Current.CancellationToken);

        Assert.Same(request, service.Added);
        Assert.Equal(request.Email, response.Email);
    }

    internal static CreateContactPerson ValidRequest() => new()
    {
        FirstName = "Ada",
        LastName = "Lovelace",
        DateOfBirth = new DateOnly(1990, 12, 10),
        Mobile = "+44 20 7946 0000",
        Email = "ada@example.com",
        City = "London",
        PostalCode = "N1 9GU",
    };

    /// <summary>Hand-written fake: the repo avoids mocking libraries with non-permissive licences (ADR 0001).</summary>
    private sealed class RecordingContactPersonService : IContactPersonService
    {
        public CreateContactPerson? Added { get; private set; }

        public Task<ContactPersonResponse> Add(CreateContactPerson createDto)
        {
            Added = createDto;
            return Task.FromResult(new ContactPersonResponse
            {
                Id = Guid.NewGuid(),
                FirstName = createDto.FirstName,
                LastName = createDto.LastName,
                DateOfBirth = createDto.DateOfBirth,
                Mobile = createDto.Mobile,
                Email = createDto.Email,
                City = createDto.City,
                PostalCode = createDto.PostalCode,
            });
        }

        public Task<ContactPersonResponse> Update(UpdateContactPerson updateDto) => throw new NotSupportedException();
        public Task<bool> Delete(Guid id) => throw new NotSupportedException();
        public Task<ContactPersonResponse> FindByID(Guid id) => throw new NotSupportedException();
        public Task<IEnumerable<ContactPersonResponse>> FindAll() => throw new NotSupportedException();
    }
}
