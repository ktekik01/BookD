namespace APIBookD.Models.Entities.Book.BookDTOs
{
    public class BookDTO
    {
        // title
        public string Title { get; set; }

        // author

        public string Author { get; set; }

        // genre
        public string Genre { get; set; }

        // description
        public string Description { get; set; }

        // publication date
        public DateTime PublicationDate { get; set; }

        // publisher
        public string Publisher { get; set; }

        // cover page image
        public string Image { get; set; }

        // number of pages

        public int NumberOfPages { get; set; }

        // ISBN number

        public string ISBN { get; set; }

        // language
        public string Language { get; set; }

    }
}
