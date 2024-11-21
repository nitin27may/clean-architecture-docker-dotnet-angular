namespace Contact.Domain.Entities;
public class User : BaseEntity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Username { get; set; }
    public required long Mobile { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}
