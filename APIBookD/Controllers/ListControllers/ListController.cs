using APIBookD.Data;
using APIBookD.Models.Entities.Book;
using APIBookD.Models.Entities.List;
using APIBookD.Models.Entities.List.ListDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIBookD.Controllers.ListControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListController : ControllerBase
    {
        private readonly BookDDbContext _context;

        public ListController(BookDDbContext context)
        {
            _context = context;
        }



        [HttpGet]
        public IActionResult GetLists([FromQuery] string searchQuery = null, [FromQuery] string listType = null, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            // Join Lists with Users based on UserId
            var query = from list in _context.Lists
                        join user in _context.Users on list.UserId equals user.Id
                        select new
                        {
                            List = list,
                            UserName = user.Name,
                            UserSurname = user.Surname
                        };

            // Filter by list type if provided
            if (!string.IsNullOrEmpty(listType))
            {
                query = query.Where(l => l.List.Type == listType);
            }

            // Search by name and surname if provided
            if (!string.IsNullOrEmpty(searchQuery))
            {
                // Split the search query into individual terms
                var searchTerms = searchQuery.Split(' ');

                foreach (var term in searchTerms)
                {
                    query = query.Where(l => l.UserName.Contains(term) || l.UserSurname.Contains(term));
                }
            }

            // Apply pagination
            var totalRecords = query.Count();
            var lists = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(l => l.List)
                .ToList();

            // Return paginated result
            return Ok(new { TotalRecords = totalRecords, Lists = lists });
        }







        [HttpGet("user/{id}")]
        public IActionResult GetListsByUserId(string id, [FromQuery] string listType = null, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (Guid.TryParse(id, out Guid userId))
            {
                var query = _context.Lists.Where(l => l.UserId == userId);

                if (!string.IsNullOrEmpty(listType))
                {
                    query = query.Where(l => l.Type == listType);
                }

                var totalRecords = query.Count();
                var lists = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return Ok(new { TotalRecords = totalRecords, Lists = lists });
            }
            else
            {
                return BadRequest("Invalid User Id");
            }
        }



        [HttpGet("contents/{id}")]
        public IActionResult GetListContents(Guid id)
        {
            // Fetch the list details
            var list = _context.Lists.FirstOrDefault(l => l.Id == id);

            if (list == null)
            {
                return NotFound("List not found");
            }

            // Fetch the user details based on the OwnerId or equivalent foreign key in the List entity
            var user = _context.Users.FirstOrDefault(u => u.Id == list.UserId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            // Fetch all ListBook relations associated with the list
            var listBooks = _context.ListBooks
                .Where(lb => lb.ListId == id)
                .Select(lb => lb.BookId)
                .ToList();

            // Fetch the names of the books associated with the list
            var bookNames = _context.Books
                .Where(b => listBooks.Contains(b.Id))
                .Select(b => b.Title)
                .ToList();

            // Create a response object that includes the list details, book names, and user details
            var response = new
            {
                Name = list.Name,
                Description = list.Description,
                Type = list.Type,
                BookNames = bookNames,
                OwnerName = $"{user.Name} {user.Surname}",
                ownerId = user.Id
            };

            return Ok(response);
        }



        /*
        [HttpGet("contents/{id}")]
        public IActionResult GetListContents(Guid id)
        {
            var listBooks = _context.ListBooks.Where(lb => lb.ListId == id).ToList();
            var response = new List<Book>();

            foreach (var listBook in listBooks)
            {
                response.Add(new Book
                {
                    Id = listBook.BookId,
                    Title = _context.Books.FirstOrDefault(b => b.Id == listBook.BookId).Title,
                    Author = _context.Books.FirstOrDefault(b => b.Id == listBook.BookId).Author,
                    Genre = _context.Books.FirstOrDefault(b => b.Id == listBook.BookId).Genre,
                    Description = _context.Books.FirstOrDefault(b => b.Id == listBook.BookId).Description,
                    PublicationDate = _context.Books.FirstOrDefault(b => b.Id == listBook.BookId).PublicationDate,
                    Publisher = _context.Books.FirstOrDefault(b => b.Id == listBook.BookId).Publisher,
                    Image = _context.Books.FirstOrDefault(b => b.Id == listBook.BookId).Image,
                    NumberOfPages = _context.Books.FirstOrDefault(b => b.Id == listBook.BookId).NumberOfPages,
                    ISBN = _context.Books.FirstOrDefault(b => b.Id == listBook.BookId).ISBN,
                    Language = _context.Books.FirstOrDefault(b => b.Id == listBook.BookId).Language,
                    AverageRating = _context.Books.FirstOrDefault(b => b.Id == listBook.BookId).AverageRating
                });
            }

            return Ok(response);
        } */

        // get wishlist of a user
        [HttpGet("wishlist/user/{id}")]
        public IActionResult GetWishlistByUserId(string id)
        {
            if (Guid.TryParse(id, out Guid userId))
            {
                var lists = _context.Lists.Where(l => l.UserId == userId && l.Type == "wishlist").ToList();
                return Ok(lists);
            }
            else
            {
                return BadRequest("Invalid User Id");
            }
        }

        // get reading lists of a user
        [HttpGet("readinglist/user/{id}")]
        public IActionResult GetReadingListByUserId(string id)
        {
            if (Guid.TryParse(id, out Guid userId))
            {
                var lists = _context.Lists.Where(l => l.UserId == userId && l.Type == "reading list").ToList();
                return Ok(lists);
            }
            else
            {
                return BadRequest("Invalid User Id");
            }
        }

        // create wishlist. parameter passes is the id of the user who creates the wishlist.
        [HttpPost("wishlist")]
        public IActionResult CreateWishlist(Guid userId)
        {
            var list = new Models.Entities.List.List
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Type = "wishlist"
            };

            _context.Lists.Add(list);
            _context.SaveChanges();

            return Ok(list);
        }

        // create reading list. parameter passed is the ListDTO object. UserId of the list is the id of the user who creates the list. it is not contained in the ListDTO object.
        [HttpPost("readinglist")]
        public IActionResult CreateReadingList(ListDTO listDTO, Guid userId)
        {
            var list = new Models.Entities.List.List
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Type = "reading list",
                Name = listDTO.Name,
                Description = listDTO.Description
            };

            _context.Lists.Add(list);
            _context.SaveChanges();

            return Ok(list);
        }

        public class AddBookRequest
        {
            public Guid ListId { get; set; }
            public string BookName { get; set; }
        }

        [HttpPost("addbook")]
        public IActionResult AddBookToList([FromBody] AddBookRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.BookName))
            {
                return BadRequest("Book name cannot be empty");
            }

            var book = _context.Books.FirstOrDefault(b => b.Title == request.BookName);

            if (book == null)
            {
                return BadRequest("Book not found");
            }

            var listBook = _context.ListBooks.FirstOrDefault(lb => lb.ListId == request.ListId && lb.BookId == book.Id);

            if (listBook != null)
            {
                return BadRequest("Book already added to the list");
            }

            listBook = new ListBook
            {
                ListId = request.ListId,
                BookId = book.Id
            };

            _context.ListBooks.Add(listBook);
            _context.SaveChanges();

            return Ok(new { Message = "Book added to the list successfully", Book = book });
        }


        public class DeleteBookRequest
        {
            public Guid ListId { get; set; }
            public string BookName { get; set; }
        }


        [HttpDelete("deletebook")]
        public IActionResult DeleteBookFromList([FromBody] DeleteBookRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.BookName))
            {
                return BadRequest("Book name cannot be empty");
            }

            var book = _context.Books.FirstOrDefault(b => b.Title == request.BookName);

            if (book == null)
            {
                return BadRequest("Book not found");
            }

            var listBook = _context.ListBooks.FirstOrDefault(lb => lb.ListId == request.ListId && lb.BookId == book.Id);

            if (listBook == null)
            {
                return BadRequest("Book is not in the list");
            }

            _context.ListBooks.Remove(listBook);
            _context.SaveChanges();

            return Ok(new { Message = "Book removed from the list successfully" });
        }



        // get the book id from the book name
        [HttpGet("book/{bookName}")]
        public IActionResult GetBookId(string bookName)
        {
            var book = _context.Books.FirstOrDefault(b => b.Title == bookName);

            if (book == null)
            {
                return NotFound("Book not found");
            }

            return Ok(book.Id);
        }


        // delete list. First remove all books from the list by deleting all ListBook relations of the list. Then delete the list.
        [HttpDelete("{id}")]
        public IActionResult DeleteList(Guid id)
        {
            var list = _context.Lists.FirstOrDefault(l => l.Id == id);

            if (list == null)
            {
                return NotFound("List not found");
            }

            var listBooks = _context.ListBooks.Where(lb => lb.ListId == id).ToList();

            foreach (var listBook in listBooks)
            {
                _context.ListBooks.Remove(listBook);
            }

            _context.Lists.Remove(list);
            _context.SaveChanges();

            return Ok(list);
        }





    }
}
