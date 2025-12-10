namespace Contact.Domain.Entities;

public class ActivityLogEntry
{
    public Guid UserId { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Activity { get; set; }
    public required string Endpoint { get; set; }
    public required string HttpMethod { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public required string IpAddress { get; set; }
    public required string UserAgent { get; set; }
}
