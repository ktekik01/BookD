namespace APIBookD.Models.Entities.Review
{
    public class VoteReview
    {
        // vote id
        public Guid Id { get; set; }

        // review id
        public Guid ReviewId { get; set; }

        // user id
        public Guid UserId { get; set; }

        // vote
        public bool Vote { get; set; }
    }
}
