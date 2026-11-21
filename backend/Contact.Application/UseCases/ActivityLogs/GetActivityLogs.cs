using Contact.Application.Abstractions.Messaging;
using Contact.Domain.Entities;

namespace Contact.Application.UseCases.ActivityLogs;

/// <summary>Activity log entries, optionally filtered by username and email (empty means no filter).</summary>
public sealed record GetActivityLogs(string Username, string Email) : IRequest<IEnumerable<ActivityLogEntry>>;
