﻿namespace Contact.Application.UseCases.Users;

public class AuthenticateResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Token { get; set; }
    public bool Authenticate { get; set; }
}
