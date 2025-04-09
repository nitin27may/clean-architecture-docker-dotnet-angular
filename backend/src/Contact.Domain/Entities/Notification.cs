namespace Contact.Domain.Entities;

public class Notification : BaseEntity
{
    public Guid UserId { get; set; }
    public string Message { get; set; }
    public bool IsRead { get; set; } = false;
}
