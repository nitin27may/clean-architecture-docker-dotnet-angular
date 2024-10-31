namespace Contact.Application.UseCases.Users;

public class UpdateUser
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public int Mobile { get; set; }
    public string Email { get; set; }
}
