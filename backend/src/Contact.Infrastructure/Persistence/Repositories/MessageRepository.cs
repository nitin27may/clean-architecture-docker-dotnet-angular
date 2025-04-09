using Contact.Domain.Entities;
using Contact.Domain.Interfaces;
using Contact.Infrastructure.Persistence.Helper;
using Dapper;
using System.Data;

namespace Contact.Infrastructure.Persistence.Repositories
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(IDapperHelper dapperHelper) : base(dapperHelper, "Messages")
        {
        }

        public async Task<IEnumerable<Message>> GetMessagesForUser(Guid userId)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("UserId", userId);
            var sql = @"
                SELECT * FROM ""Messages""
                WHERE ""UserId"" = @UserId
                ORDER BY ""CreatedOn"" DESC;";
            return await _dapperHelper.GetAll<Message>(sql, dbPara);
        }
    }
}
