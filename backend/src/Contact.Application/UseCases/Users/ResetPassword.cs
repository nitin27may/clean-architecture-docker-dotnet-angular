namespace Contact.Application.UseCases.Users;

public class ResetPassword
{
    public required string Email { get; init; }
    public required string ResetCode { get; init; }
    public required string NewPassword { get; init; }
}
