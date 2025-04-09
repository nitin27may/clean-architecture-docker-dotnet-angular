using Contact.Domain.Entities;
using Contact.Domain.Interfaces;
using Contact.Infrastructure.Persistence.Helper;
using Dapper;
using System.Data;

namespace Contact.Infrastructure.Persistence.Repositories;

public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
{
    public NotificationRepository(IDapperHelper dapperHelper) : base(dapperHelper, "Notifications")
    {
    }

    public async Task<IEnumerable<Notification>> GetUserNotifications(Guid userId)
    {
        var dbPara = new DynamicParameters();
        dbPara.Add("UserId", userId);
        var sql = @"
            SELECT * FROM ""Notifications""
            WHERE ""UserId"" = @UserId
            ORDER BY ""CreatedOn"" DESC;";
        return await _dapperHelper.GetAll<Notification>(sql, dbPara);
    }

    public async Task MarkAsRead(Guid userId, Guid notificationId)
    {
        var dbPara = new DynamicParameters();
        dbPara.Add("UserId", userId);
        dbPara.Add("NotificationId", notificationId);
        var sql = @"
            UPDATE ""Notifications""
            SET ""IsRead"" = TRUE
            WHERE ""UserId"" = @UserId AND ""Id"" = @NotificationId;";
        await _dapperHelper.Execute(sql, dbPara);
    }
}
