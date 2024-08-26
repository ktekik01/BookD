namespace APIBookD.Models.Entities.Book
{
    public class BookRates
    {
        //id
        public Guid Id { get; set; }

        //book id
        public Guid BookId { get; set; }

        //rating
        public float Rating { get; set; }

        //user id
        public Guid UserId { get; set; }
    }
}
