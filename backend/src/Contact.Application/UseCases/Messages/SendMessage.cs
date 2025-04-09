namespace Contact.Application.UseCases.Messages
{
    public class SendMessage
    {
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
