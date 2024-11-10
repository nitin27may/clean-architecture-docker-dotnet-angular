﻿using Contact.Application.UseCases.Users;
using Contact.Domain.Entities;
using System.Security.Claims;

namespace Contact.Application.Interfaces;


public interface IUserService
{
    Task<User> Create(CreateUser createUser);
    Task<User> Add(RegisterUser user);
    Task<bool> Delete(Guid id);
    Task<UserResponse> Update(UpdateUser user);
    Task<UserResponse> FindByID(Guid id);
    Task<IEnumerable<User>> CheckUniqueUsers(string email, string username);
    Task<IEnumerable<UserResponse>> FindAll();
    Task<AuthenticateResponse> Authenticate(AuthenticateRequest authenticateRequest);
    Guid GetUserId(ClaimsPrincipal user);
    Task<IEnumerable<string>> GetUserRolesAsync(ClaimsPrincipal user);

    Task<bool> ChangePassword(ChangePassword changePassword);
    Task<bool> ForgotPassword(string email);
    Task<bool> ResetPassword(ResetPassword resetPasswordRequest);
}
