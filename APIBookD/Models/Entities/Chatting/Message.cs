namespace APIBookD.Models.Entities.Chatting
{
    public class Message
    {

        // message id
        public Guid Id { get; set; }

        // chat id
        public Guid ChatId { get; set; }

        // sender id
        public Guid SenderId { get; set; }

        // receiver id
        public Guid ReceiverId { get; set; }

        // message content
        public string Content { get; set; }

        // message time
        public DateTime Time { get; set; }
    }
}
