namespace APIBookD.Models.Entities.Review
{
    public class Review
    {

        //review id
        public Guid Id { get; set; }

        // review title
        public string Title { get; set; }

        // user id
        public Guid UserId { get; set; }

        // book id
        public Guid BookId { get; set; }

        // review text

        public string ReviewText { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }


    }
}
