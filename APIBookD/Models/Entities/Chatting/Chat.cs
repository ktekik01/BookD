namespace APIBookD.Models.Entities.Chatting
{
    public class Chat
    {

        // chat id
        public Guid Id { get; set; }

        // users list
        public List<Guid>? UsersList { get; set; }

        // messages in chat
        public List<Guid>? Messages { get; set; }
    }
}
