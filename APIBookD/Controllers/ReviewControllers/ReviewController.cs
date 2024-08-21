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


        // upvote a review. If the user has already upvoted the review, remove the upvote. If the user has downvoted the review, remove the downvote and add the upvote.
        // If the user has not upvoted the review, add the upvote.
        // add user id to upvotes list of a review.
        // then, add user id and review id to VoteReview object.

        [HttpPost("upvote")]

        public IActionResult UpvoteReview(Guid reviewId, Guid userId)
        {
               
            // check if the user has already upvoted the review
            var review = _context.Reviews.Find(reviewId);
            if (review.Upvotes.Contains(userId))
            {
                review.Upvotes.Remove(userId);
                _context.SaveChanges();
                return Ok("Upvote removed");
            }
            else
            {
                // check if the user has downvoted the review
                if (review.Downvotes.Contains(userId))
                {
                    review.Downvotes.Remove(userId);

                    // remove from VoteReview

                    var vote = _context.VoteReviews.FirstOrDefault(v => v.ReviewId == reviewId && v.UserId == userId);
                    _context.VoteReviews.Remove(vote);

                    review.Upvotes.Add(userId);

                    // add to VoteReview

                    var newVote = new Models.Entities.Review.VoteReview
                    {
                        Id = Guid.NewGuid(),
                        ReviewId = reviewId,
                        UserId = userId,
                        Vote = true // means upvote
                    };

                    _context.SaveChanges();

                    return Ok("Downvote removed and upvote added");
                }
                else
                {
                    review.Upvotes.Add(userId);

                    // add to VoteReview

                    var vote = new Models.Entities.Review.VoteReview
                    {
                        Id = Guid.NewGuid(),
                        ReviewId = reviewId,
                        UserId = userId,
                        Vote = true // means upvote
                    };

                    _context.SaveChanges();
                    return Ok("Upvote added");
                }
            }


        }

        // downvote a review. If the user has already downvoted the review, remove the downvote. If the user has upvoted the review, remove the upvote and add the downvote.
        // If the user has not downvoted the review, add the downvote.
        // add user id to downvotes list of a review.
        // then, add user id and review id to VoteReview object.

        [HttpPost("downvote")]

        public IActionResult DownvoteReview(Guid reviewId, Guid userId)
        {
            // check if the user has already downvoted the review
            var review = _context.Reviews.Find(reviewId);
            if (review.Downvotes.Contains(userId))
            {
                review.Downvotes.Remove(userId);
                _context.SaveChanges();
                return Ok("Downvote removed");
            }
            else
            {
                // check if the user has upvoted the review
                if (review.Upvotes.Contains(userId))
                {
                    review.Upvotes.Remove(userId);

                    // remove from VoteReview

                    var vote = _context.VoteReviews.FirstOrDefault(v => v.ReviewId == reviewId && v.UserId == userId);
                    _context.VoteReviews.Remove(vote);

                    review.Downvotes.Add(userId);

                    // add to VoteReview

                    var newVote = new Models.Entities.Review.VoteReview
                    {
                        Id = Guid.NewGuid(),
                        ReviewId = reviewId,
                        UserId = userId,
                        Vote = false // means downvote
                    };

                    _context.SaveChanges();

                    return Ok("Upvote removed and downvote added");
                }
                else
                {
                    review.Downvotes.Add(userId);

                    // add to VoteReview

                    var vote = new Models.Entities.Review.VoteReview
                    {
                        Id = Guid.NewGuid(),
                        ReviewId = reviewId,
                        UserId = userId,
                        Vote = false // means downvote
                    };

                    _context.SaveChanges();
                    return Ok("Downvote added");
                }
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

    }
}
