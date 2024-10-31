namespace Contact.Domain.Entities;

public class UpdatePassword : BaseEntity
{
    public required string CurrentPassword { get; set; }
    public required string NewPassword { get; set; }
}
