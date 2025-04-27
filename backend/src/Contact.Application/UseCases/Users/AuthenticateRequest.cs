using System.ComponentModel.DataAnnotations;

namespace Contact.Application.UseCases.Users;

public class AuthenticateRequest
{
    [Required]
    public required string Username { get; set; }
    
    [Required]
    public required string Password { get; set; }
}
