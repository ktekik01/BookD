namespace APIBookD.Models.Entities.Review.ReviewDTOs
{
    public class ReviewWithBookInfoDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        public string ReviewText { get; set; }
        // upvotes list
        public List<Guid> Upvotes { get; set; }

        // downvotes list

        public List<Guid> Downvotes { get; set; }
        public DateTime ReviewDate { get; set; }
        public string BookTitle { get; set; }
        public string BookAuthor { get; set; }
    }
}
