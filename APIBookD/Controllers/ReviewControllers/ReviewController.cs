using APIBookD.Data;
using APIBookD.Models.Entities.Review;
using APIBookD.Models.Entities.Review.ReviewDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> GetReviews()
        {
            var reviews = await _context.Reviews.ToListAsync();

            var response = new List<Review>();

            foreach (var review in reviews)
            {
                response.Add(new Review
                {
                    Id = review.Id,
                    Title = review.Title,
                    UserId = review.UserId,
                    BookId = review.BookId,
                    ReviewText = review.ReviewText,
                    Upvotes = review.Upvotes,
                    Downvotes = review.Downvotes,
                    ReviewDate = review.ReviewDate
                });
            }

            return Ok(response);
        }

        // get all reviews of a user. From the bookIds of the review, get the book titles and authors.

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetReviewsByUserId(string id)
        {
            if (Guid.TryParse(id, out Guid userId))
            {
                // Retrieve reviews for the user. While retrieving, the bookId is received.
                // Also retrieve the book details.

                var reviews = await _context.Reviews.Where(r => r.UserId == userId).ToListAsync();
                var response = new List<ReviewWithBookInfoDTO>();

                foreach (var review in reviews)
                    {
                    var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == review.BookId);
                    response.Add(new ReviewWithBookInfoDTO
                    {
                        Id = review.Id,
                        Title = review.Title,
                        UserId = review.UserId,
                        BookId = review.BookId,
                        ReviewText = review.ReviewText,
                        Upvotes = review.Upvotes,
                        Downvotes = review.Downvotes,
                        ReviewDate = review.ReviewDate,
                        BookTitle = book.Title,
                        BookAuthor = book.Author
                    });
                }

                return Ok(response);
            }
            else
            {
                return BadRequest("Invalid User Id");
            }
        }


        /*
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetReviewsByUserId(string id)
        {
            if (Guid.TryParse(id, out Guid userId))
            {
                var reviews = await _context.Reviews.Where(r => r.UserId == userId).ToListAsync();

                var response = new List<Review>();

                foreach (var review in reviews)
                {
                    response.Add(new Review
                    {
                        Id = review.Id,
                        Title = review.Title,
                        UserId = review.UserId,
                        BookId = review.BookId,
                        ReviewText = review.ReviewText,
                        Upvotes = review.Upvotes,
                        Downvotes = review.Downvotes,
                        ReviewDate = review.ReviewDate
                    });
                }

                return Ok(response);
            }
            else
            {
                return BadRequest("Invalid User Id");
            }
        }*/
        // get review by id
        [HttpGet("{id}")]
        public IActionResult GetReviewById(Guid id)
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
            // check if the user exists
            var user = _context.Reviewers.Find(reviewDTO.UserId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // check if the book exists
            var book = _context.Books.Find(reviewDTO.BookId);
            if (book == null)
            {
                return NotFound("Book not found");
            }

            // add the review to the database

            var review = new Models.Entities.Review.Review
            {
                Id = Guid.NewGuid(),
                Title = reviewDTO.Title,
                UserId = reviewDTO.UserId,
                BookId = reviewDTO.BookId,
                ReviewText = reviewDTO.ReviewText,
                Upvotes = new List<Guid>(),
                Downvotes = new List<Guid>(),
                ReviewDate = DateTime.Now
            };

            _context.Reviews.Add(review);
            _context.SaveChanges();

            return Ok(review);
        }

        // add a comment to a review
        [HttpPost("comment")]

        public IActionResult AddCommentToReview(Guid reviewId, Guid userId, string Content)
        {
            // add the comment to the database

            var comment = new Models.Entities.Review.CommentToReview
            {
                Id = Guid.NewGuid(),
                ReviewId = reviewId,
                UserId = userId,
                Content = Content,
                CommentDate = DateTime.Now
            };

            _context.CommentToReviews.Add(comment);
            _context.SaveChanges();

            return Ok(comment);
        }


        [HttpPost("upvote")]
        public IActionResult UpvoteReview(Guid reviewId, Guid userId)
        {
            var review = _context.Reviews.Find(reviewId);
            if (review == null)
            {
                return NotFound("Review not found");
            }

            var reviewer = _context.Reviewers.Find(userId);
            if (reviewer == null)
            {
                return NotFound("Reviewer not found");
            }

            if (review.Upvotes.Contains(userId))
            {
                review.Upvotes.Remove(userId);
                reviewer.UpvotedReviews.Remove(reviewId);

                var existingVote = _context.VoteReviews.FirstOrDefault(v => v.ReviewId == reviewId && v.UserId == userId);
                if (existingVote != null)
                {
                    _context.VoteReviews.Remove(existingVote);
                }

                _context.SaveChanges();
                return Ok("Upvote removed");
            }
            else
            {
                if (review.Downvotes.Contains(userId))
                {
                    review.Downvotes.Remove(userId);
                    reviewer.DownvotedReviews.Remove(reviewId);

                    var existingVote = _context.VoteReviews.FirstOrDefault(v => v.ReviewId == reviewId && v.UserId == userId);
                    if (existingVote != null)
                    {
                        _context.VoteReviews.Remove(existingVote);
                    }
                }

                review.Upvotes.Add(userId);
                reviewer.UpvotedReviews.Add(reviewId);

                var newVote = new Models.Entities.Review.VoteReview
                {
                    Id = Guid.NewGuid(),
                    ReviewId = reviewId,
                    UserId = userId,
                    Vote = true // means upvote
                };

                _context.VoteReviews.Add(newVote);
                _context.SaveChanges();
                return Ok("Upvote added");
            }
        }


        [HttpPost("downvote")]
        public IActionResult DownvoteReview(Guid reviewId, Guid userId)
        {
            var review = _context.Reviews.Find(reviewId);
            if (review == null)
            {
                return NotFound("Review not found");
            }

            var reviewer = _context.Reviewers.Find(userId);
            if (reviewer == null)
            {
                return NotFound("Reviewer not found");
            }

            if (review.Downvotes.Contains(userId))
            {
                review.Downvotes.Remove(userId);
                reviewer.DownvotedReviews.Remove(reviewId);

                var existingVote = _context.VoteReviews.FirstOrDefault(v => v.ReviewId == reviewId && v.UserId == userId);
                if (existingVote != null)
                {
                    _context.VoteReviews.Remove(existingVote);
                }

                _context.SaveChanges();
                return Ok("Downvote removed");
            }
            else
            {
                if (review.Upvotes.Contains(userId))
                {
                    review.Upvotes.Remove(userId);
                    reviewer.UpvotedReviews.Remove(reviewId);

                    var existingVote = _context.VoteReviews.FirstOrDefault(v => v.ReviewId == reviewId && v.UserId == userId);
                    if (existingVote != null)
                    {
                        _context.VoteReviews.Remove(existingVote);
                    }
                }

                review.Downvotes.Add(userId);
                reviewer.DownvotedReviews.Add(reviewId);

                var newVote = new Models.Entities.Review.VoteReview
                {
                    Id = Guid.NewGuid(),
                    ReviewId = reviewId,
                    UserId = userId,
                    Vote = false // means downvote
                };

                _context.VoteReviews.Add(newVote);
                _context.SaveChanges();
                return Ok("Downvote added");
            }
        }



        // delete a review. First remove all comments to the review, then remove all up and downvotes to the review, then remove the review.

        [HttpDelete("{id}")]

        public IActionResult DeleteReview(string id)
        {
            if (Guid.TryParse(id, out Guid reviewId))
            {
                var review = _context.Reviews.Find(reviewId);
                if (review != null)
                {
                    var comments = _context.CommentToReviews.Where(c => c.ReviewId == reviewId).ToList();
                    foreach (var comment in comments)
                    {
                        _context.CommentToReviews.Remove(comment);
                    }

                    var votes = _context.VoteReviews.Where(v => v.ReviewId == reviewId).ToList();
                    foreach (var vote in votes)
                    {
                        _context.VoteReviews.Remove(vote);
                    }

                    _context.Reviews.Remove(review);
                    _context.SaveChanges();
                    return Ok("Review deleted");
                }
                else
                {
                    return BadRequest("Review not found");
                }
            }
            else
            {
                return BadRequest("Invalid Review Id");
            }
        }

        // update a review. Only the review text can be updated.

        [HttpPut("{id}")]
        public IActionResult UpdateReview(string id, ReviewDTO reviewDTO)
        {
            if (Guid.TryParse(id, out Guid reviewId))
            {
                var review = _context.Reviews.Find(reviewId);
                if (review != null)
                {
                    review.ReviewText = reviewDTO.ReviewText;
                    _context.SaveChanges();
                    return Ok(review);
                }
                else
                {
                    return BadRequest("Review not found");
                }
            }
            else
            {
                return BadRequest("Invalid Review Id");
            }
        }

        // get all the review that a user has upvoted and downvoted
        [HttpGet("user/review/voted/{id}")]
        public IActionResult GetVotedReviewsByUserId(string id)
        {
            if (Guid.TryParse(id, out Guid userId))
            {
                var upvotedReviews = _context.VoteReviews.Where(v => v.UserId == userId && v.Vote == true).ToList();
                var downvotedReviews = _context.VoteReviews.Where(v => v.UserId == userId && v.Vote == false).ToList();
                return Ok(new { upvotedReviews, downvotedReviews });
            }
            else
            {
                return BadRequest("Invalid User Id");
            }
        }

        // get all upvotes and downvotes of a review
        [HttpGet("votes/{id}")]
        public IActionResult GetVotesOfReview(string id)
        {
            if (Guid.TryParse(id, out Guid reviewId))
            {
                var review = _context.Reviews.Find(reviewId);
                if (review != null)
                {
                    var upvotes = _context.VoteReviews.Where(v => v.ReviewId == reviewId && v.Vote == true).ToList();
                    var downvotes = _context.VoteReviews.Where(v => v.ReviewId == reviewId && v.Vote == false).ToList();
                    return Ok(new { upvotes, downvotes });
                }
                else
                {
                    return BadRequest("Review not found");
                }
            }
            else
            {
                return BadRequest("Invalid Review Id");
            }
        }

        // get all comments to a review
        [HttpGet("comments/{id}")]
        public IActionResult GetCommentsToReview(string id)
        {
            if (Guid.TryParse(id, out Guid reviewId))
            {
                var comments = _context.CommentToReviews.Where(c => c.ReviewId == reviewId).ToList();
                return Ok(comments);
            }
            else
            {
                return BadRequest("Invalid Review Id");
            }
        }


        // get all comments that a user has made
        [HttpGet("comments/user/{id}")]
        public IActionResult GetCommentsByUserId(string id)
        {
            if (Guid.TryParse(id, out Guid userId))
            {
                var comments = _context.CommentToReviews.Where(c => c.UserId == userId).ToList();
                return Ok(comments);
            }
            else
            {
                return BadRequest("Invalid User Id");
            }
        }

        // delete a comment
        [HttpDelete("comment/{id}")]
        public IActionResult DeleteComments(string id) {
            if (Guid.TryParse(id, out Guid commentId))
            {
                var comment = _context.CommentToReviews.Find(commentId);
                if (comment != null)
                {
                    _context.CommentToReviews.Remove(comment);
                    _context.SaveChanges();
                    return Ok("Comment deleted");
                }
                else
                {
                    return BadRequest("Comment not found");
                }
            }
            else
            {
                return BadRequest("Invalid Comment Id");
            }
        }

        // sort reviews by date
        [HttpGet("sort/date")]
        public IActionResult SortReviewsByDate()
        {
            var reviews = _context.Reviews.OrderByDescending(r => r.ReviewDate).ToList();
            return Ok(reviews);
        }

        // sort reviews by upvotes number in descending order
        [HttpGet("sort/upvotes")]
        public IActionResult SortReviewsByUpvotes()
        {
            var reviews = _context.Reviews.OrderByDescending(r => r.Upvotes.Count).ToList();
            return Ok(reviews);
        }

        // sort reviews by downvotes number in descending order
        [HttpGet("sort/downvotes")]
        public IActionResult SortReviewsByDownvotes()
        {
            var reviews = _context.Reviews.OrderByDescending(r => r.Downvotes.Count).ToList();
            return Ok(reviews);
        }

        // sort reviews by the number of comments in descending order
        [HttpGet("sort/comments")]
        public IActionResult SortReviewsByComments()
        {
            var reviews = _context.Reviews.OrderByDescending(r => _context.CommentToReviews.Where(c => c.ReviewId == r.Id).Count()).ToList();
            return Ok(reviews);
        }

    }

}
