using Contact.Application.Abstractions.Messaging;
using System.ComponentModel.DataAnnotations;

namespace Contact.Application.UseCases.Users;

/// <summary>Sign-in (POST /api/Users/authenticate).</summary>
public class AuthenticateRequest : IRequest<AuthenticateResponse>
{
    [Required]
    public required string Username { get; set; }
    
    [Required]
    public required string Password { get; set; }
}
