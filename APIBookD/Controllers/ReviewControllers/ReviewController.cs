using APIBookD.Data;
using APIBookD.Models.Entities.Review.ReviewDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIBookD.Controllers.ReviewControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {

        private readonly BookDDbContext _context;

        public ReviewController(BookDDbContext context)
        {
            _context = context;
        }

        // get all reviews
        [HttpGet]
        public IActionResult GetReviews()
        {
            var reviews = _context.Reviews.ToList();
            return Ok(reviews);
        }

        // get review by id
        [HttpGet("{id}")]
        public IActionResult GetReviewById(string id)
        {
            var review = _context.Reviews.Find(id);
            return Ok(review);
        }

        // get all reviews of a book
        [HttpGet("book/{id}")]
        public IActionResult GetReviewsByBookId(string id)
        {
            if (Guid.TryParse(id, out Guid bookId))
            {
                var reviews = _context.Reviews.Where(r => r.BookId == bookId).ToList();
                return Ok(reviews);
            }
            else
            {
                return BadRequest("Invalid Book Id");
            }
        }

        // add review
        [HttpPost]

        public IActionResult AddReview(ReviewDTO reviewDTO)
        {
            // add the review to the database

            var review = new Models.Entities.Review.Review
            {
                Id = Guid.NewGuid(),
                Title = reviewDTO.Title,
                UserId = reviewDTO.UserId,
                BookId = reviewDTO.BookId,
                ReviewText = reviewDTO.ReviewText,
                Upvotes = reviewDTO.Upvotes,
                Downvotes = reviewDTO.Downvotes
            };

            _context.Reviews.Add(review);
            _context.SaveChanges();

            return Ok(review);
        }
    }
}
