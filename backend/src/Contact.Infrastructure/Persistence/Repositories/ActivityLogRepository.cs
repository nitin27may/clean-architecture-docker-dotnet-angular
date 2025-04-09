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
        dbPara.Add("Username", logEntry.Username);
        dbPara.Add("Email", logEntry.Email);
        dbPara.Add("Activity", logEntry.Activity);
        dbPara.Add("Endpoint", logEntry.Endpoint);
        dbPara.Add("HttpMethod", logEntry.HttpMethod);
        dbPara.Add("Timestamp", logEntry.Timestamp);
        dbPara.Add("IpAddress", logEntry.IpAddress);
        dbPara.Add("UserAgent", logEntry.UserAgent);
        var sql = @"
            INSERT INTO ""ActivityLog"" (""UserId"", ""Username"", ""Email"", ""Activity"", ""Endpoint"", ""HttpMethod"", ""Timestamp"", ""IpAddress"", ""UserAgent"")
            VALUES (@UserId, @Username, @Email, @Activity, @Endpoint, @HttpMethod, @Timestamp, @IpAddress, @UserAgent)
            RETURNING *";

        await _dapperHelper.Insert<ActivityLogEntry>(sql, dbPara, CommandType.Text);
    }

    public async Task<IEnumerable<ActivityLogEntry>> GetActivityLogsAsync(string username, string email)
    {
        var dbPara = new DynamicParameters();
        dbPara.Add("Username", username);
        dbPara.Add("Email", email);
        var sql = @"
            SELECT * FROM ""ActivityLog""
            WHERE (""Username"" = @Username OR @Username IS NULL)
            OR (""Email"" = @Email OR @Email IS NULL)";

        return await _dapperHelper.GetAll<ActivityLogEntry>(sql, dbPara, CommandType.Text);
    }
}
