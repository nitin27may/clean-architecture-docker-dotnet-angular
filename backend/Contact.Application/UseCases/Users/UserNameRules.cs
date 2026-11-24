namespace Contact.Application.UseCases.Users;

/// <summary>Allowed username characters, shared by register, create and update.</summary>
internal static class UserNameRules
{
    // '@' is allowed because the app uses email addresses as usernames (see the seed data).
    public const string Pattern = @"^[a-zA-Z0-9._@+-]+$";
    public const string Message = "Username can only contain letters, numbers and . _ @ + -";
}
