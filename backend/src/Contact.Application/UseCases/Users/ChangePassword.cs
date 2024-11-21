namespace Contact.Application.UseCases.Users;

public class ChangePassword
{
    public required string CurrentPassword { get; set; }
    public required string NewPassword { get; set; }
}
