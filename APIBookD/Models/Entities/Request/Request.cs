namespace APIBookD.Models.Entities.Request
{
    public class Request
    {

        // request id
        public Guid Id { get; set; }

        // user id
        public Guid UserId { get; set; }

        // request title
        public string Title { get; set; }

        // request content
        public string Content { get; set; }

        // request date
        public DateTime RequestDate { get; set; }

        // request status
        public string Status { get; set; }
    }
}
