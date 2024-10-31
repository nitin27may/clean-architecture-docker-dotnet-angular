using System.ComponentModel.DataAnnotations;

namespace Contact.Application.UseCases.Users;

public class AuthenticateRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}
