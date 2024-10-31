using Contact.Domain.Entities;
using Contact.Domain.Interfaces;
using Contact.Infrastructure.Persistence.Helper;
using Dapper;
using System.Data;

namespace Contact.Infrastructure.Persistence.Repositories;

public class ActivityLogRepository : IActivityLogRepository
{
    protected readonly IDapperHelper _dapperHelper;
    public ActivityLogRepository(IDapperHelper dapperHelper)
    {
        _dapperHelper = dapperHelper ?? throw new ArgumentException(nameof(dapperHelper));
    }

    public async Task LogActivityAsync(ActivityLogEntry logEntry)
    {
        var dbPara = new DynamicParameters();
        dbPara.Add("UserId", logEntry.UserId);
        dbPara.Add("Activity", logEntry.Activity);
        dbPara.Add("Endpoint", logEntry.Endpoint);
        dbPara.Add("HttpMethod", logEntry.HttpMethod);
        dbPara.Add("Timestamp", logEntry.Timestamp);
        dbPara.Add("IpAddress", logEntry.IpAddress);
        dbPara.Add("UserAgent", logEntry.UserAgent);
        var sql = @"
            INSERT INTO ActivityLog (UserId, Activity, Endpoint, HttpMethod, Timestamp, IpAddress, UserAgent)
            VALUES (@UserId, @Activity, @Endpoint, @HttpMethod, @Timestamp, @IpAddress, @UserAgent)"
        ;


        await _dapperHelper.Insert<ActivityLogEntry>(sql, dbPara, CommandType.Text);
    }
}
