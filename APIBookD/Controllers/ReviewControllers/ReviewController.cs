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


        /*
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
        } */


        /*
        [HttpGet]
        public async Task<IActionResult> GetReviews(string? title, string? user, string? book, int page = 1, int pageSize = 10, string? sortBy = "reviewDate", bool SortDescending = false)
        {


                    var query = _context.Reviews
            .Include(r => r.User) // Include related User
            .Include(r => r.Book) // Include related Book
            .AsQueryable();

            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(b => b.Title.ToLower().Contains(title.ToLower()));
            }

            if (!string.IsNullOrEmpty(user))
            {

                // review does not contain user name and surname, contains the user id. to get the user name and surname
                // from the user id, first get the user from the user id, then get the name and surname from the user.

                query = query.Where(b => b.UserId.ToString().ToLower().Contains(user.ToLower()));
            }

            if (!string.IsNullOrEmpty(book))
            {
                // review does not contain book title, contains the book id. to get the book title
                // from the book id, first get the book from the book id, then get the title from the book.

                query = query.Where(b => b.BookId.ToString().ToLower().Contains(book.ToLower()));
            }


            if (SortDescending)
            {
                query = sortBy switch
                {
                    "reviewDate" => query.OrderByDescending(b => b.ReviewDate),
                    "upvotes" => query.OrderByDescending(b => b.Upvotes.Count),
                    "downvotes" => query.OrderByDescending(b => b.Downvotes.Count),
                    _ => query.OrderByDescending(b => b.ReviewDate)
                };
            }
            else
            {
                query = sortBy switch
                {
                    "reviewDate" => query.OrderBy(b => b.ReviewDate),
                    "upvotes" => query.OrderBy(b => b.Upvotes.Count),
                    "downvotes" => query.OrderBy(b => b.Downvotes.Count),
                    _ => query.OrderBy(b => b.ReviewDate)
                };
            }

            // pagination
            var totalReviews = await query.CountAsync();
            var reviews = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var response = new
            {
                TotalReviews = totalReviews,
                Reviews = reviews
            };

            return Ok(response);
        } */


        /*
        [HttpGet]
        public async Task<IActionResult> GetReviews(string? title, string? user, string? book, int page = 1, int pageSize = 10, string? sortBy = "reviewDate", bool sortDescending = false)
        {
            var reviewsQuery = _context.Reviews.AsQueryable();

            // Apply pagination
            var reviews = await reviewsQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // Fetch related Book and User entities in a single query
            var bookIds = reviews.Select(r => r.BookId).Distinct().ToList();
            var userIds = reviews.Select(r => r.UserId).Distinct().ToList();

            var books = await _context.Books.Where(b => bookIds.Contains(b.Id)).ToDictionaryAsync(b => b.Id, b => b.Title);
            var users = await _context.Users.Where(u => userIds.Contains(u.Id)).ToDictionaryAsync(u => u.Id, u => u.Name + " " + u.Surname);

            // Apply filtering
            if (!string.IsNullOrEmpty(title))
            {
                reviews = reviews.Where(r => books.TryGetValue(r.BookId, out var bookTitle) && bookTitle.ToLower().Contains(title.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(user))
            {
                reviews = reviews.Where(r => users.TryGetValue(r.UserId, out var userName) && userName.ToLower().Contains(user.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(book))
            {
                reviews = reviews.Where(r => books.TryGetValue(r.BookId, out var bookTitle) && bookTitle.ToLower().Contains(book.ToLower())).ToList();
            }

            // Apply sorting
            reviews = sortDescending
                ? sortBy switch
                {
                    "reviewDate" => reviews.OrderByDescending(r => r.ReviewDate).ToList(),
                    "upvotes" => reviews.OrderByDescending(r => r.Upvotes.Count).ToList(),
                    "downvotes" => reviews.OrderByDescending(r => r.Downvotes.Count).ToList(),
                    _ => reviews.OrderByDescending(r => r.ReviewDate).ToList(),
                }
                : sortBy switch
                {
                    "reviewDate" => reviews.OrderBy(r => r.ReviewDate).ToList(),
                    "upvotes" => reviews.OrderBy(r => r.Upvotes.Count).ToList(),
                    "downvotes" => reviews.OrderBy(r => r.Downvotes.Count).ToList(),
                    _ => reviews.OrderBy(r => r.ReviewDate).ToList(),
                };

            // Prepare response
            var totalReviews = await _context.Reviews.CountAsync();
            var response = new
            {
                TotalReviews = totalReviews,
                Reviews = reviews
            };

            return Ok(response);
        }
        */

        [HttpGet]
        public async Task<IActionResult> GetReviews(string? title, string? user, string? book, int page = 1, int pageSize = 10, string? sortBy = "reviewDate", bool sortDescending = false)
        {
            var query = _context.Reviews.AsQueryable();

            // Apply filtering
            if (!string.IsNullOrEmpty(title))
            {
                var bookIds = await _context.Books
                    .Where(b => b.Title.ToLower().Contains(title.ToLower()))
                    .Select(b => b.Id)
                    .ToListAsync();
                query = query.Where(r => bookIds.Contains(r.BookId));
            }

            if (!string.IsNullOrEmpty(user))
            {
                var userIds = await _context.Users
                    .Where(u => (u.Name + " " + u.Surname).ToLower().Contains(user.ToLower()))
                    .Select(u => u.Id)
                    .ToListAsync();
                query = query.Where(r => userIds.Contains(r.UserId));
            }

            if (!string.IsNullOrEmpty(book))
            {
                var bookIds = await _context.Books
                    .Where(b => b.Title.ToLower().Contains(book.ToLower()))
                    .Select(b => b.Id)
                    .ToListAsync();
                query = query.Where(r => bookIds.Contains(r.BookId));
            }

            // Apply sorting
            if (sortDescending)
            {
                query = sortBy switch
                {
                    "reviewDate" => query.OrderByDescending(r => r.ReviewDate),
                    "upvotes" => query.OrderByDescending(r => r.Upvotes.Count),
                    "downvotes" => query.OrderByDescending(r => r.Downvotes.Count),
                    _ => query.OrderByDescending(r => r.ReviewDate)
                };
            }
            else
            {
                query = sortBy switch
                {
                    "reviewDate" => query.OrderBy(r => r.ReviewDate),
                    "upvotes" => query.OrderBy(r => r.Upvotes.Count),
                    "downvotes" => query.OrderBy(r => r.Downvotes.Count),
                    _ => query.OrderBy(r => r.ReviewDate)
                };
            }

            // Pagination
            var totalReviews = await query.CountAsync();
            var reviews = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // Fetch book and user details
            var reviewDetails = reviews.Select(r => new
            {
                r.Id,
                r.Title,
                r.ReviewText,
                r.ReviewDate,
                r.UserId,
                BookTitle = _context.Books.Where(b => b.Id == r.BookId).Select(b => b.Title).FirstOrDefault(),
                UserName = _context.Users.Where(u => u.Id == r.UserId).Select(u => u.Name + " " + u.Surname).FirstOrDefault(),
                r.Upvotes,
                r.Downvotes
            }).ToList();

            var response = new
            {
                TotalReviews = totalReviews,
                Reviews = reviewDetails
            };

            return Ok(response);
        }




        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetReviewsByUserId(string id, string? title, string? book, int page = 1, int pageSize = 10, string? sortBy = "reviewDate", bool sortDescending = false)
        {
            if (Guid.TryParse(id, out Guid userId))
            {
                var query = _context.Reviews.Where(r => r.UserId == userId).AsQueryable();

                // Apply filtering
                if (!string.IsNullOrEmpty(title))
                {
                    var bookIds = await _context.Books
                        .Where(b => b.Title.ToLower().Contains(title.ToLower()))
                        .Select(b => b.Id)
                        .ToListAsync();
                    query = query.Where(r => bookIds.Contains(r.BookId));
                }

                if (!string.IsNullOrEmpty(book))
                {
                    var bookIds = await _context.Books
                        .Where(b => b.Title.ToLower().Contains(book.ToLower()))
                        .Select(b => b.Id)
                        .ToListAsync();
                    query = query.Where(r => bookIds.Contains(r.BookId));
                }

                // Apply sorting
                query = sortDescending
                    ? sortBy switch
                    {
                        "reviewDate" => query.OrderByDescending(r => r.ReviewDate),
                        "upvotes" => query.OrderByDescending(r => r.Upvotes.Count),
                        "downvotes" => query.OrderByDescending(r => r.Downvotes.Count),
                        _ => query.OrderByDescending(r => r.ReviewDate)
                    }
                    : sortBy switch
                    {
                        "reviewDate" => query.OrderBy(r => r.ReviewDate),
                        "upvotes" => query.OrderBy(r => r.Upvotes.Count),
                        "downvotes" => query.OrderBy(r => r.Downvotes.Count),
                        _ => query.OrderBy(r => r.ReviewDate)
                    };

                // Pagination
                var totalReviews = await query.CountAsync();
                var reviews = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

                return Ok(new
                {
                    TotalReviews = totalReviews,
                    Reviews = reviews
                });
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
        /*
        [HttpGet("{id}")]
        public IActionResult GetReviewById(Guid id)
        {
            var review = _context.Reviews.Find(id);
            return Ok(review);
        } */

        // get review by id. also return the book title by using the book id in the review to reach the book table
        // and get the user name and surname of the user using user id to reach User table.

        [HttpGet("{id}")]
        public IActionResult GetReviewById(string id)
        {
            if (Guid.TryParse(id, out Guid reviewId))
            {
                var review = _context.Reviews.Find(reviewId);
                if (review != null)
                {
                    var bookTitle = _context.Books.Where(b => b.Id == review.BookId).Select(b => b.Title).FirstOrDefault();
                    var userName = _context.Reviewers.Where(u => u.Id == review.UserId).Select(u => u.Name + " " + u.Surname).FirstOrDefault();
                    return Ok(new { review, bookTitle, userName });
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





        // users adds a review with a title, review text, book title. while adding the review,
        // find the book id from the book title and add it to the Review object.

        [HttpPost]

        public IActionResult AddReview(ReviewDTO reviewDTO)
        {
            // check if the user exists
            var user = _context.Reviewers.Find(reviewDTO.UserId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // find the book id from the book title. If the book does not exist, return an error message.

            var book = _context.Books.FirstOrDefault(b => b.Title == reviewDTO.BookTitle);


            // add the review to the database

            var review = new Models.Entities.Review.Review
            {
                Id = Guid.NewGuid(),
                Title = reviewDTO.Title,
                UserId = reviewDTO.UserId,
                BookId = book.Id,
                ReviewText = reviewDTO.ReviewText,
                Upvotes = new List<Guid>(),
                Downvotes = new List<Guid>(),
                ReviewDate = DateTime.Now
            };

            // output error message if title is not found
            if (book == null)
            {
                return NotFound("Title not found");
            }

            // output error message if userid is not found
            if (user == null)
            {
                return NotFound("User not found");
            }

            // output error message if reviewText is not found
            if (reviewDTO.ReviewText == null)
            {
                return NotFound("Review text not found");
            }

            _context.Reviews.Add(review);
            _context.SaveChanges();

            return Ok(review);
        }


        /*
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
        } */

        public class CommentDto
        {
            public Guid ReviewId { get; set; }
            public Guid UserId { get; set; }
            public string Content { get; set; }
        }


        // add a comment to a review
        [HttpPost("comment")]

        public IActionResult AddCommentToReview(CommentDto commentDto)
        {
            // add the comment to the database

            // check whether parameters are valid
            var review = _context.Reviews.Find(commentDto.ReviewId);
            if (review == null) {
                return NotFound("Review not found");
            }

            var user = _context.Reviewers.Find(commentDto.UserId);
            if (user == null) {
                return NotFound("User not found");
            }
            if (review == null) {
                return NotFound("Review not found");
            }


            var comment = new Models.Entities.Review.CommentToReview
            {
                Id = Guid.NewGuid(),
                ReviewId = commentDto.ReviewId,
                UserId = commentDto.UserId,
                Content = commentDto.Content,
                CommentDate = DateTime.Now
            };

            _context.CommentToReviews.Add(comment);
            _context.SaveChanges();

            return Ok(comment);
        }

        public class UpvoteRequestModel
        {
            public Guid ReviewId { get; set; }
            public Guid UserId { get; set; }
        }


        [HttpPost("upvote")]
        public IActionResult UpvoteReview([FromBody] UpvoteRequestModel model)
        {

            var review = _context.Reviews.Find(model.ReviewId);
            if (review == null)
            {
                return NotFound(new { message = "Review not found" });
            }

            var reviewer = _context.Reviewers.Find(model.UserId);
            if (reviewer == null)
            {
                return NotFound(new { message = "Reviewer not found" });
            }

            if (review.Upvotes.Contains(model.UserId))
            {
                review.Upvotes.Remove(model.UserId);
                reviewer.UpvotedReviews.Remove(model.ReviewId);
                

                var existingVote = _context.VoteReviews.FirstOrDefault(v => v.ReviewId == model.ReviewId && v.UserId == model.UserId);
                if (existingVote != null)
                {
                    _context.VoteReviews.Remove(existingVote);
                }

                _context.SaveChanges();
                return Ok(new { message = "Upvote removed" });
            }
            else
            {
                if (review.Downvotes.Contains(model.UserId))
                {
                    review.Downvotes.Remove(model.UserId);
                    reviewer.DownvotedReviews.Remove(model.ReviewId);

                    var existingVote = _context.VoteReviews.FirstOrDefault(v => v.ReviewId == model.ReviewId && v.UserId == model.UserId);
                    if (existingVote != null)
                    {
                        _context.VoteReviews.Remove(existingVote);
                    }
                }

                review.Upvotes.Add(model.UserId);
                reviewer.UpvotedReviews.Add(model.ReviewId);

                var newVote = new VoteReview
                {
                    Id = Guid.NewGuid(),
                    ReviewId = model.ReviewId,
                    UserId = model.UserId,
                    Vote = true // means upvote
                };

                _context.VoteReviews.Add(newVote);
                _context.SaveChanges();
                return Ok(new { message = "Upvote added" });
            }
        }





        public class DownvoteRequestModel
        {
            public Guid ReviewId { get; set; }
            public Guid UserId { get; set; }
        }

        [HttpPost("downvote")]
        public IActionResult DownvoteReview([FromBody] DownvoteRequestModel model)
        {
            if (model.ReviewId == Guid.Empty || model.UserId == Guid.Empty)
            {
                return BadRequest("Invalid input data");
            }

            var review = _context.Reviews.Find(model.ReviewId);
            if (review == null)
            {
                return NotFound("Review not found");
            }

            var reviewer = _context.Reviewers.Find(model.UserId);
            if (reviewer == null)
            {
                return NotFound("Reviewer not found");
            }

            if (review.Downvotes.Contains(model.UserId))
            {
                review.Downvotes.Remove(model.UserId);
                reviewer.DownvotedReviews.Remove(model.ReviewId);

                var existingVote = _context.VoteReviews.FirstOrDefault(v => v.ReviewId == model.ReviewId && v.UserId == model.UserId);
                if (existingVote != null)
                {
                    _context.VoteReviews.Remove(existingVote);
                }

                _context.SaveChanges();
                return Ok("Downvote removed");
            }
            else
            {
                if (review.Upvotes.Contains(model.UserId))
                {
                    review.Upvotes.Remove(model.UserId);
                    reviewer.UpvotedReviews.Remove(model.ReviewId);

                    var existingVote = _context.VoteReviews.FirstOrDefault(v => v.ReviewId == model.ReviewId && v.UserId == model.UserId);
                    if (existingVote != null)
                    {
                        _context.VoteReviews.Remove(existingVote);
                    }
                }

                review.Downvotes.Add(model.UserId);
                reviewer.DownvotedReviews.Add(model.ReviewId);

                var newVote = new VoteReview
                {
                    Id = Guid.NewGuid(),
                    ReviewId = model.ReviewId,
                    UserId = model.UserId,
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
