namespace APIBookD.Models.Entities.Review
{
    public class CommentToReview
    {


        // comment id
        public Guid Id { get; set; }

        // review id
        public Guid ReviewId { get; set; }

        // user id
        public Guid UserId { get; set; }

        // comment content
        public string Content { get; set; }

        // comment date
        public DateTime CommentDate { get; set; }
    }
}
