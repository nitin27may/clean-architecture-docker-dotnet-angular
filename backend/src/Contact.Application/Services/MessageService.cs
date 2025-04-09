using Contact.Application.Interfaces;
using Contact.Application.UseCases.Messages;
using Contact.Domain.Entities;
using Contact.Domain.Interfaces;

namespace Contact.Application.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task SendMessage(SendMessage sendMessage)
        {
            var message = new Message
            {
                Id = Guid.NewGuid(),
                UserId = sendMessage.UserId,
                Content = sendMessage.Content,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = sendMessage.CreatedBy
            };

            await _messageRepository.Add(message);
        }

        public async Task<IEnumerable<MessageResponse>> GetMessagesForUser(Guid userId)
        {
            var messages = await _messageRepository.GetMessagesForUser(userId);
            return messages.Select(m => new MessageResponse
            {
                Id = m.Id,
                UserId = m.UserId,
                Content = m.Content,
                CreatedOn = m.CreatedOn,
                CreatedBy = m.CreatedBy
            });
        }
    }
}
