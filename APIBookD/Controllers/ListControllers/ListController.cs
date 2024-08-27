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

        // get all lists
        [HttpGet]
        public IActionResult GetLists()
        {
            var lists = _context.Lists.ToList();
            return Ok(lists);
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
                OwnerName = $"{user.Name} {user.Surname}"
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

        // add book to an existing list. This happens via using ListBook relation. 
        [HttpPost("addbook")]
        public IActionResult AddBookToList(Guid listId, Guid bookId)
        {
            var listBook = new Models.Entities.List.ListBook
            {
                ListId = listId,
                BookId = bookId
            };

            _context.ListBooks.Add(listBook);
            _context.SaveChanges();

            return Ok(listBook);
        }

        // remove book from a list. This happens via using ListBook relation.
        [HttpDelete("removebook")]
        public IActionResult RemoveBookFromList(Guid listId, Guid bookId)
        {
            var listBook = _context.ListBooks.Find(listId, bookId);

            if (listBook == null)
            {
                return NotFound("ListBook not found");
            }

            _context.ListBooks.Remove(listBook);
            _context.SaveChanges();

            return Ok(listBook);
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
