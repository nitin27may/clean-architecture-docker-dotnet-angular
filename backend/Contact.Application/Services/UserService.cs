using AutoMapper;
using Contact.Application.Interfaces;
using Contact.Application.UseCases.RolePermissions;
using Contact.Application.UseCases.Roles;
using Contact.Application.UseCases.Users;
using Contact.Domain.Entities;
using Contact.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Contact.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly ILogger<UserService> _logger;
    private readonly IMapper _mapper;
    private readonly AppSettings _appSettings;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    private readonly IRolePermissionService _rolePermissionService;

    public UserService(IUserRepository userRepository,
        IRoleRepository roleRepository,
        IUserRoleRepository userRoleRepository,
        ILogger<UserService> logger,
        IOptions<AppSettings> appSettings,
        IMapper mapper,
         IUnitOfWork unitOfWork,
         IEmailService emailService,
         IRolePermissionService rolePermissionService)
    {
        _appSettings = appSettings.Value;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _userRoleRepository = userRoleRepository;
        _logger = logger;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
        _rolePermissionService = rolePermissionService;
    }

    public async Task<User> Add(RegisterUser createUser)
    {
        PasswordHasher<string> passwordHasher = PasswordHash();
        var hashedPassword = passwordHasher.HashPassword(createUser.Email, createUser.Password);
        createUser.Password = hashedPassword;

        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            var user = _mapper.Map<User>(createUser);
            var createdUser = await _userRepository.Add(user, transaction);
            var role = await _roleRepository.GetRoleByName("Admin", transaction);
            var userRole = new UserRole
            {
                UserId = createdUser.Id,
                RoleId = role.Id,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = createdUser.Id
            };
            await _userRoleRepository.Add(userRole, transaction);
            await _unitOfWork.CommitAsync();
            return createdUser;
        }
        catch (Exception)
        {
            // Rollback the transaction if something goes wrong
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<User> Create(CreateUser createUser)
    {
        PasswordHasher<string> passwordHasher = PasswordHash();

        var hashedPassword = passwordHasher.HashPassword(createUser.Email, "New Password");
        var user = _mapper.Map<User>(createUser);
        user.Password = hashedPassword;

        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            var createdUser = await _userRepository.Add(user, transaction);
            var role = await _roleRepository.GetRoleByName(createUser.Role, transaction);
            var userRole = new UserRole
            {
                UserId = createdUser.Id,
                RoleId = role.Id,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = createdUser.Id
            };
            await _userRoleRepository.Add(userRole, transaction);
            await _unitOfWork.CommitAsync();
            await CompleteRegistration(createdUser);
            return createdUser;
        }
        catch (Exception)
        {
            // Rollback the transaction if something goes wrong
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> Delete(Guid id)
    {
        return await _userRepository.Delete(id);
    }

    public async Task<IEnumerable<UserResponse>> FindAll()
    {
        var users = await _userRepository.FindAll();
        return _mapper.Map<IEnumerable<UserResponse>>(users);
    }

    public async Task<UserResponse> FindByID(Guid id)
    {
        var user = await _userRepository.FindByID(id);
        return _mapper.Map<UserResponse>(user);
    }

    public async Task<UserResponse> Update(UpdateUser updateUser)
    {
        var user = _mapper.Map<User>(updateUser);
        var result = await _userRepository.Update(user);
        return _mapper.Map<UserResponse>(result);
    }

    public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest authenticateRequest)
    {
        var user = await _userRepository.FindByUserName(authenticateRequest.Username);
        if (user == null)
        {
            var failedResponse = new AuthenticateResponse()
            {
                Authenticate = false
            };
            return failedResponse;
        }
        var passwordHasher = new PasswordHasher<string>();
        var result = passwordHasher.VerifyHashedPassword(user.Email, user.Password, authenticateRequest.Password);
        var token = await GenerateJwtToken(user);

        var rolePermissions = await _rolePermissionService.GetRolePermissionMappingsAsync(user.Id);
        var rolePermissionResponse = _mapper.Map<List<RolePermissionResponse>>(rolePermissions.ToList());
        var authenticateResponse = new AuthenticateResponse()
        {
            Authenticate = true,
            Token = token,
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            RolePermissions = rolePermissionResponse
        };
        if (result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded)
        {
            return authenticateResponse;
        }
        else if (result == PasswordVerificationResult.Failed)
        {
            var failedResponse = new AuthenticateResponse()
            {
                Authenticate = false
            };
            return failedResponse;
        }
        return authenticateResponse;
    }

    private async Task<string> GenerateJwtToken(User user)
    {
        // generate token that is valid for 7 days
        var tokenHandler = new JwtSecurityTokenHandler();
        var roles = await _userRepository.FindRolesById(user.Id);
        var identityClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.FirstName + " "+ user.LastName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("Id", user.Id.ToString()),
        };
        foreach (var role in roles)
        {
            identityClaims.Add(new Claim(ClaimTypes.Role, role.Name));
        }
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _appSettings.Issuer,
            audience: _appSettings.Audience,
            claims: identityClaims,
            expires: DateTime.UtcNow.AddHours(1), // Set token expiration as needed
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public Guid GetUserId(ClaimsPrincipal user)
    {
        return Guid.Parse(user.FindFirst("Id")?.Value);
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync(ClaimsPrincipal user)
    {
        var roles = user.FindAll(ClaimTypes.Role)
                        .Select(c => c.Value)
                        .ToList(); // Or ToArray() if you need an array
        return roles;
    }

    public async Task<IEnumerable<User>> CheckUniqueUsers(string email, string username)
    {
        return await _userRepository.CheckUniqueUsers(email, username);
    }

    public async Task<bool> ChangePassword(ChangePassword changePassword)
    {
        var updatePassword = _mapper.Map<UpdatePassword>(changePassword);
        var user = await _userRepository.FindByID(updatePassword.UpdatedBy.GetValueOrDefault()); // pass logged in user id here

        if (user == null)
        {
            _logger.LogWarning("User with ID {userId} not found.", updatePassword.Id);
            return false;
        }

        // Validate the current password
        var passwordHasher = new PasswordHasher<string>();
        var result = passwordHasher.VerifyHashedPassword(user.Email, user.Password, updatePassword.CurrentPassword);

        if (result != PasswordVerificationResult.Success)
        {
            _logger.LogWarning("Invalid current password for user with ID {userId}.", updatePassword.Id);
            return false;
        }

        // Hash the new password and update it
        var hashedPassword = passwordHasher.HashPassword(user.Email, updatePassword.NewPassword);
        user.Password = hashedPassword;
        user.UpdatedOn = DateTime.UtcNow;
        user.UpdatedBy = updatePassword.UpdatedBy.GetValueOrDefault();

        await _userRepository.UpdatePassword(user);

        _logger.LogInformation("Password successfully changed for user with email {email}.", user.Email);
        return true;
    }

    public async Task<bool> ForgotPassword(string email)
    {
        var user = await _userRepository.FindByUserName(email);
        if (user == null)
        {
            _logger.LogWarning("User with email {email} not found.", email);
            return false; // User not found
        }
        var resetLink = await GenerateLink(user);
        var emailSubject = "Password Reset Request";
        var emailBody = $"Click the link to reset your password: {resetLink}";

        // Assume there's an IEmailService to handle email sending
        await _emailService.SendEmailAsync(user.Email, emailSubject, emailBody);

        _logger.LogInformation("Password reset email sent to {email}.", user.Email);
        return true;
    }

    public async Task<bool> ResetPassword(ResetPassword resetPasswordRequest)
    {
        // Step 1: Validate the reset token // token send as part of email
        var userId = ValidatePasswordResetToken(resetPasswordRequest.ResetCode);
        if (userId == Guid.Empty)
        {
            _logger.LogWarning("Invalid password reset token.");
            return false;
        }

        // Step 2: Find the user by their ID
        var user = await _userRepository.FindByID(userId);
        if (user == null)
        {
            _logger.LogWarning("User with ID {userId} not found.", userId);
            return false;
        }

        // Step 3: Hash the new password
        var passwordHasher = new PasswordHasher<string>();
        var hashedPassword = passwordHasher.HashPassword(user.Email, resetPasswordRequest.NewPassword);

        // Step 4: Update the password
        user.Password = hashedPassword;
        user.UpdatedOn = DateTime.UtcNow;

        await _userRepository.Update(user);

        _logger.LogInformation("Password successfully reset for user with email {email}.", user.Email);
        return true;
    }

    private Guid ValidatePasswordResetToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_appSettings.Secret);

        try
        {
            var claims = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _appSettings.Issuer,
                ValidAudience = _appSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = true // Ensure the token is not expired
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;

            return Guid.Parse(userId);
        }
        catch (Exception ex)
        {
            _logger.LogError("Token validation failed: {error}", ex.Message);
            return Guid.Empty;
        }
    }

    private string GeneratePasswordResetToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _appSettings.Issuer,
            audience: _appSettings.Audience,
            claims: new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim("Email", user.Email)
            },
            expires: DateTime.UtcNow.AddHours(1), // Token valid for 1 hour
            signingCredentials: creds
        );

        return tokenHandler.WriteToken(token);
    }

    private async Task<bool> CompleteRegistration(User user)
    {
        var resetLink = await GenerateLink(user);
        var emailSubject = "Complete Registration";
        var emailBody = $"Click the link to Complete your registration and set password: {resetLink}";

        // Assume there's an IEmailService to handle email sending
        await _emailService.SendEmailAsync(user.Email, emailSubject, emailBody);

        _logger.LogInformation("Password reset email sent to {email}.", user.Email);
        return true;
    }

    private async Task<string> GenerateLink(User user)
    {
        // Step 2: Generate a password reset token (e.g., JWT token with expiration)
        var token = GeneratePasswordResetToken(user);

        // Step 3: Send the token to the user's email
        var resetLink = $"{_appSettings.PasswordResetUrl}?token={token}";
        return resetLink;
    }

    private static PasswordHasher<string> PasswordHash()
    {
        return new PasswordHasher<string>(
            new OptionsWrapper<PasswordHasherOptions>(
                new PasswordHasherOptions()
                {
                    CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2
                })
        );
    }

    public async Task<UserResponse> GetUserWithPermissionsAsync(Guid userId)
    {
        var user = await _userRepository.FindByID(userId);
        if (user == null) return null;

        var rolePermissions = await _rolePermissionService.GetRolePermissionMappingsAsync(userId);

        var userResponse = _mapper.Map<UserResponse>(user);
        userResponse.RolePermissions = rolePermissions.ToList();
        return userResponse;
    }

    public async Task<UserResponse> UpdateUserRoles(Guid userId, UpdateUserRoles updateUserRoles)
    {
        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            // Get the current user
            var user = await _userRepository.FindByID(userId);
            if (user == null)
            {
                _logger.LogWarning("User with ID {userId} not found.", userId);
                return null;
            }

            // Delete all existing user roles
            var existingUserRoles = await _userRepository.GetUserRoles(userId, transaction);
            foreach (var userRole in existingUserRoles)
            {
                await _userRoleRepository.Delete(userRole.Id, transaction);
            }

            // Add new user roles
            foreach (var roleId in updateUserRoles.RoleIds)
            {
                if (Guid.TryParse(roleId, out Guid parsedRoleId))
                {
                    var userRole = new UserRole
                    {
                        UserId = userId,
                        RoleId = parsedRoleId,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = userId // Using the user's own ID as creator
                    };
                    await _userRoleRepository.Add(userRole, transaction);
                }
            }

            await _unitOfWork.CommitAsync();

            // Return updated user with permissions
            return await GetUserWithPermissionsAsync(userId);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error updating roles for user {userId}", userId);
            throw;
        }
    }

    public async Task<UserWithRolesResponse> GetUserWithRolesAsync(Guid userId)
    {
        var user = await _userRepository.FindByID(userId);
        if (user == null) return null;

        // Get the user's roles
        var roles = await _userRepository.FindRolesById(userId);

        // Map to response
        var userResponse = _mapper.Map<UserWithRolesResponse>(user);
        userResponse.RolePermissions = null; // Keep rolePermissions as null as requested
        userResponse.Roles = _mapper.Map<List<RoleResponse>>(roles);

        return userResponse;
    }
}