using Contact.Application.UseCases.Messages;

namespace Contact.Application.Interfaces
{
    public interface IMessageService
    {
        Task SendMessage(SendMessage sendMessage);
        Task<IEnumerable<MessageResponse>> GetMessagesForUser(Guid userId);
    }
}
