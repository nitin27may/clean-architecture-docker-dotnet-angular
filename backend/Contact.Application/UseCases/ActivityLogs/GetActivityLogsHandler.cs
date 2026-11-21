using Contact.Application.Abstractions.Messaging;
using Contact.Application.Interfaces;
using Contact.Domain.Entities;

namespace Contact.Application.UseCases.ActivityLogs;

public sealed class GetActivityLogsHandler(IActivityLogService activityLogService)
    : IRequestHandler<GetActivityLogs, IEnumerable<ActivityLogEntry>>
{
    public Task<IEnumerable<ActivityLogEntry>> Handle(GetActivityLogs request, CancellationToken cancellationToken)
        => activityLogService.GetActivityLogsAsync(request.Username?.Trim() ?? "", request.Email?.Trim() ?? "");
}
