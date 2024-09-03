using APIBookD.Data;
using APIBookD.Models.Entities.Book;
using APIBookD.Models.Entities.Book.BookDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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


        [HttpGet]
        public async Task<IActionResult> GetBooks(string? title, string? author, string? publisher, string? genre, string? sortBy = "publicationDate", bool sortDescending = false, int page = 1, int pageSize = 10)
        {
            var query = _context.Books.AsQueryable();

            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(b => b.Title.ToLower().Contains(title.ToLower()));
            }

            if (!string.IsNullOrEmpty(author))
            {
                query = query.Where(b => b.Author.ToLower().Contains(author.ToLower()));
            }

            if (!string.IsNullOrEmpty(publisher))
            {
                query = query.Where(b => b.Publisher.ToLower().Contains(publisher.ToLower()));
            }

            if (!string.IsNullOrEmpty(genre))
            {
                query = query.Where(b => b.Genre.ToLower().Contains(genre.ToLower()));
            }

            // Sorting, eight options: publication date ascending descending, average rating ascending descending, number of pages ascending descending, number of reviews ascending descending

            if (sortBy == "publicationDateDesc")
            {
                query = sortDescending ? query.OrderByDescending(b => b.PublicationDate) : query.OrderBy(b => b.PublicationDate);
            }
            else if(sortBy == "publicationDateAsc") 
            { 
                query = sortDescending ? query.OrderBy(b => b.PublicationDate) : query.OrderByDescending(b => b.PublicationDate);
            }
            else if(sortBy == "averageRatingDesc") 
            { 
                query = sortDescending ? query.OrderByDescending(b => b.AverageRating) : query.OrderBy(b => b.AverageRating);
            }
            else if(sortBy == "averageRatingAsc") 
            { 
                query = sortDescending ? query.OrderBy(b => b.AverageRating) : query.OrderByDescending(b => b.AverageRating);
            }
            else if(sortBy == "numberOfPagesDesc") 
            { 
                query = sortDescending ? query.OrderByDescending(b => b.NumberOfPages) : query.OrderBy(b => b.NumberOfPages);
            }
            else if(sortBy == "numberOfPagesAsc") 
            { 
                query = sortDescending ? query.OrderBy(b => b.NumberOfPages) : query.OrderByDescending(b => b.NumberOfPages);
            }
            else if(sortBy == "numberOfReviewsDesc") 
            { 
                query = sortDescending ? query.OrderByDescending(b => _context.Reviews.Where(r => r.BookId == b.Id).Count()) : query.OrderBy(b => _context.Reviews.Where(r => r.BookId == b.Id).Count());
            }
            else if(sortBy == "numberOfReviewsAsc") 
            { 
                query = sortDescending ? query.OrderBy(b => _context.Reviews.Where(r => r.BookId == b.Id).Count()) : query.OrderByDescending(b => _context.Reviews.Where(r => r.BookId == b.Id).Count());
            }

            // Pagination
            var totalBooks = await query.CountAsync();
            var books = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var response = new
            {
                TotalBooks = totalBooks,
                Books = books
            };

            return Ok(response);
        }


        /*
        // get all books
        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            //var books = _context.Books.ToList();
            //return Ok(books);

            var books = await _context.Books.ToListAsync();

            var response = new List<Book>();

            foreach (var book in books)
            {
                response.Add(new Book{
                    Id = book.Id,
                        Title = book.Title,
                        Author = book.Author,
                        Genre = book.Genre,
                        Description = book.Description,
                        PublicationDate = book.PublicationDate,
                        Publisher = book.Publisher,
                        Image = book.Image,
                        NumberOfPages = book.NumberOfPages,
                        ISBN = book.ISBN,
                        Language = book.Language,
                        AverageRating = book.AverageRating
                });
            }

            return Ok(response);
        }

        */

        // get book by id
        [HttpGet("{id}")]
        public IActionResult GetBookById(Guid id)
        {
            var book = _context.Books.Find(id);

            if (book == null)
            {
                return BadRequest("The book does not exist.");
            }

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



        // rate a book. If the book does not exist, return a message saying that the book does not exist.
        // if the user has already rated the book and tries to rate it again, change the rating to the new rating.
        // update the average rating of the book.

        [HttpPost("rate/{id}")]
        public IActionResult RateBook(Guid id, Guid userId, float rating)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var book = _context.Books.FirstOrDefault(b => b.Id == id);

                    if (book == null)
                    {
                        return BadRequest("The book does not exist.");
                    }

                    var rate = _context.BookRates.FirstOrDefault(r => r.BookId == id && r.UserId == userId);

                    if (rate == null)
                    {
                        var newRate = new BookRates
                        {
                            Id = Guid.NewGuid(),
                            BookId = id,
                            Rating = rating,
                            UserId = userId
                        };

                        _context.BookRates.Add(newRate);
                    }
                    else
                    {
                        rate.Rating = rating;
                    }

                    // Save changes before recalculating to ensure data is up-to-date
                    _context.SaveChanges();

                    // Re-fetch the data to get the most recent state
                    var rates = _context.BookRates.Where(b => b.BookId == id).ToList();
                    double sum = rates.Sum(r => r.Rating);

                    book.AverageRating = rates.Count == 0 ? 0 : sum / rates.Count;

                    _context.SaveChanges();

                    transaction.Commit();

                    return Ok(book);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
        }



        [HttpDelete("rate/{id}")]
        public IActionResult DeleteRate(Guid id, Guid userId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var book = _context.Books.FirstOrDefault(b => b.Id == id);

                    if (book == null)
                    {
                        return BadRequest("The book does not exist.");
                    }

                    var rate = _context.BookRates.FirstOrDefault(r => r.BookId == id && r.UserId == userId);

                    if (rate == null)
                    {
                        return BadRequest("The user has not rated the book.");
                    }

                    _context.BookRates.Remove(rate);

                    // Save changes before recalculating to ensure data is up-to-date
                    _context.SaveChanges();

                    // Re-fetch the ratings to get the most recent state
                    var rates = _context.BookRates.Where(b => b.BookId == id).ToList();
                    double sum = rates.Sum(r => r.Rating);

                    book.AverageRating = rates.Count == 0 ? 0 : sum / rates.Count;

                    _context.SaveChanges();

                    transaction.Commit();

                    return Ok(book);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
        }



        // get all the ratings of a book. If the book does not exist, return a message saying that the book does not exist.
        [HttpGet("rates/{id}")]
        public IActionResult GetRatesByBookId(string id)
        {
            if (Guid.TryParse(id, out Guid bookId))
            {
                var book = _context.Books.FirstOrDefault(b => b.Id == bookId);

                if (book == null)
                {
                    return BadRequest("The book does not exist.");
                }

                var rates = _context.BookRates.Where(r => r.BookId == bookId).ToList();
                return Ok(rates);
            }
            else
            {
                return BadRequest("Invalid Book Id");
            }
        }

        // get all ratings done in the system
        [HttpGet("rates")]
        public IActionResult GetRates()
        {
            var rates = _context.BookRates.ToList();
            return Ok(rates);
        }

        // sort books by average rating in descending order
        [HttpGet("sort/rating")]
        public IActionResult SortBooksByRating()
        {
            var books = _context.Books.OrderByDescending(b => b.AverageRating).ToList();
            return Ok(books);
        }
        

        // sort books by publication date in descending order

        [HttpGet("sort/date")]
        public IActionResult SortBooksByDate()
        {
            var books = _context.Books.OrderByDescending(b => b.PublicationDate).ToList();
            return Ok(books);
        }

        // sort books by number of pages in descending order
        [HttpGet("sort/pages")]
        public IActionResult SortBooksByPages()
        {
            var books = _context.Books.OrderByDescending(b => b.NumberOfPages).ToList();
            return Ok(books);
        }

        // sort books by number of reviews writed in descending order
        [HttpGet("sort/reviews")]
        public IActionResult SortBooksByReviews()
        {
            var books = _context.Books.OrderByDescending(b => _context.Reviews.Where(r => r.BookId == b.Id).Count()).ToList();
            return Ok(books);
        }


        // get all the reviews of a book. If the book does not exist, return a message saying that the book does not exist.
        [HttpGet("reviews/{id}")]
        public IActionResult GetReviewsByBookId(string id)
        {
            if (Guid.TryParse(id, out Guid bookId))
            {
                var book = _context.Books.FirstOrDefault(b => b.Id == bookId);

                if (book == null)
                {
                    return BadRequest("The book does not exist.");
                }

                var reviews = _context.Reviews.Where(r => r.BookId == bookId).ToList();
                return Ok(reviews);
            }
            else
            {
                return BadRequest("Invalid Book Id");
            }
        }




    }
}
