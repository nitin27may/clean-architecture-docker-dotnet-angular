using Contact.Domain.Entities;

namespace Contact.Domain.Interfaces
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        Task<IEnumerable<Message>> GetMessagesForUser(Guid userId);
    }
}
