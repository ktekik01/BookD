using APIBookD.Data;
using APIBookD.Models.Entities.Book;
using APIBookD.Models.Entities.Book.BookDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIBookD.Controllers.BookControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {

        private readonly BookDDbContext _context;

        public BookController(BookDDbContext context)
        {
            _context = context;
        }

        // get all books
        [HttpGet]
        public IActionResult GetBooks()
        {
            var books = _context.Books.ToList();
            return Ok(books);
        }

        // get book by id
        [HttpGet("{id}")]
        public IActionResult GetBookById(string id)
        {
            var book = _context.Books.Find(id);
            return Ok(book);
        }

        // add book, check if the book is already in the database by checking the ISBN number. If a book with the same ISBN exists,
        // return a message saying that the book is already in the database. If not, add the book to the database.
        [HttpPost]
        public IActionResult AddBook(BookDTO bookDTO)
        {

            // check if the book is already in the database by checking the ISBN number
            var check = _context.Books.FirstOrDefault(b => b.ISBN == bookDTO.ISBN);

            if (check != null)
            {
                return BadRequest("The book is already in the database.");
            }

            // add the book to the database

            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = bookDTO.Title,
                Author = bookDTO.Author,
                Genre = bookDTO.Genre,
                Description = bookDTO.Description,
                PublicationDate = bookDTO.PublicationDate,
                Publisher = bookDTO.Publisher,
                Image = bookDTO.Image,
                NumberOfPages = bookDTO.NumberOfPages,
                ISBN = bookDTO.ISBN,
                Language = bookDTO.Language,
                AverageRating = 0
            };

            _context.Books.Add(book);
            _context.SaveChanges();
            return Ok(book);

        }


        // update book. If a field is not updated, this is not an error and the old value is kept.
        [HttpPut("{id}")]

        public IActionResult updateBook(Guid id, BookDTO bookDTO)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return BadRequest("The book does not exist.");
            }

            if (bookDTO.Title != null)
            {
                book.Title = bookDTO.Title;
            }

            if (bookDTO.Author != null)
            {
                book.Author = bookDTO.Author;
            }

            if (bookDTO.Genre != null)
            {
                book.Genre = bookDTO.Genre;
            }

            if (bookDTO.Description != null)
            {
                book.Description = bookDTO.Description;
            }

            if (bookDTO.PublicationDate != null)
            {
                book.PublicationDate = bookDTO.PublicationDate;
            }

            if (bookDTO.Publisher != null)
            {
                book.Publisher = bookDTO.Publisher;
            }

            if (bookDTO.Image != null)
            {
                book.Image = bookDTO.Image;
            }

            if (bookDTO.NumberOfPages != 0)
            {
                book.NumberOfPages = bookDTO.NumberOfPages;
            }

            if (bookDTO.ISBN != null)
            {
                book.ISBN = bookDTO.ISBN;
            }

            if (bookDTO.Language != null)
            {
                book.Language = bookDTO.Language;
            }

            _context.SaveChanges();
            return Ok(book);
        }


        // delete book
        [HttpDelete("{id}")]
        public IActionResult DeleteBook(Guid id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return BadRequest("The book does not exist.");
            }

            _context.Books.Remove(book);
            _context.SaveChanges();
            return Ok("The book has been deleted.");

        }

        // get all books by genre. handle upper and lower case. If the genre does not exist, return a message saying that the genre does not exist.
        [HttpGet("genre/{genre}")]
        public IActionResult GetBooksByGenre(string genre)
        {
            var books = _context.Books.Where(b => b.Genre.ToLower() == genre.ToLower()).ToList();

            if (books.Count == 0)
            {
                return BadRequest("The genre does not exist.");
            }

            return Ok(books);
        }

        // get all books by author. handle upper and lower case. If the author does not exist, return a message saying that the author does not exist.
        [HttpGet("author/{author}")]
        public IActionResult GetBooksByAuthor(string author)
        {
            var books = _context.Books.Where(b => b.Author.ToLower() == author.ToLower()).ToList();

            if (books.Count == 0)
            {
                return BadRequest("The author does not exist.");
            }

            return Ok(books);
        }

        // get all books by publisher. handle upper and lower case. If the publisher does not exist, return a message saying that the publisher does not exist.
        [HttpGet("publisher/{publisher}")]
        public IActionResult GetBooksByPublisher(string publisher)
        {
            var books = _context.Books.Where(b => b.Publisher.ToLower() == publisher.ToLower()).ToList();

            if (books.Count == 0)
            {
                return BadRequest("The publisher does not exist.");
            }

            return Ok(books);
        }

        // get all books by language. handle upper and lower case. If the language does not exist, return a message saying that the language does not exist.
        [HttpGet("language/{language}")]
        public IActionResult GetBooksByLanguage(string language)
        {
            var books = _context.Books.Where(b => b.Language.ToLower() == language.ToLower()).ToList();

            if (books.Count == 0)
            {
                return BadRequest("The language does not exist.");
            }

            return Ok(books);
        }



    }
}
