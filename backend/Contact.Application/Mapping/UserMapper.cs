using Contact.Application.UseCases.Users;
using Contact.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Contact.Application.Mapping;

[Mapper]
public partial class UserMapper
{
    // CreateUser carries no Password — UserService.Create hashes and assigns it itself
    // immediately after this call returns, exactly as the AutoMapper version did.
    public partial User ToUser(CreateUser source, DateTimeOffset createdOn, Guid createdBy, string password);

    // RegisterUser.Username (lowercase "n") and User.UserName differ only by casing.
    // AutoMapper's default case-insensitive convention matching papered over this; Mapperly's
    // exact-name matching correctly flags it, so it is wired explicitly here.
    [MapProperty(nameof(RegisterUser.Username), nameof(User.UserName))]
    public partial User ToUser(RegisterUser source, DateTimeOffset createdOn, Guid createdBy);

    // BaseEntity.CreatedOn/CreatedBy are required, but UpdateUser never carries them, and
    // GenericRepository.GenerateUpdateQuery excludes both columns from the UPDATE statement —
    // same story for Password: UserRepository's custom Update() override excludes that column
    // too. All three placeholders here are compiler-satisfying only and are never persisted.
    public partial User ToUser(UpdateUser source, DateTimeOffset createdOn, Guid createdBy, string password);

    public partial UserResponse ToUserResponse(User source);

    public partial UserWithRolesResponse ToUserWithRolesResponse(User source);

    // UpdatePassword also derives from BaseEntity, so it has the same required-field quirk;
    // see the UpdateUser overload above for why placeholders are safe here too.
    public partial UpdatePassword ToUpdatePassword(ChangePassword source, DateTimeOffset createdOn, Guid createdBy);
}
