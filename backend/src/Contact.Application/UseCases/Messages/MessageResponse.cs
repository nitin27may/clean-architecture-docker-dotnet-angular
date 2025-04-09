namespace Contact.Application.UseCases.Messages
{
    public class MessageResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
