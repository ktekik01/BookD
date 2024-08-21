using APIBookD.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIBookD.Controllers.FollowControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowController : ControllerBase
    {
        private readonly BookDDbContext _context;

        public FollowController(BookDDbContext context)
        {
            _context = context;
        }

        // get all followers of a user
        [HttpGet("user/{id}")]
        public IActionResult GetFollowersByUserId(string id)
        {
            if (Guid.TryParse(id, out Guid userId))
            {
                var followers = _context.Follows.Where(f => f.FollowedId == userId).ToList();
                return Ok(followers);
            }
            else
            {
                return BadRequest("Invalid User Id");
            }
        }

        // get all users followed by a user
        [HttpGet("user/{id}")]
        public IActionResult GetFollowingsByUserId(string id)
        {
            if (Guid.TryParse(id, out Guid userId))
            {
                var followings = _context.Follows.Where(f => f.FollowerId == userId).ToList();
                return Ok(followings);
            }
            else
            {
                return BadRequest("Invalid User Id");
            }
        }

        //count the number of followers of a user
        [HttpGet("user/{id}")]
        public IActionResult GetFollowersCountByUserId(string id)
        {
            if (Guid.TryParse(id, out Guid userId))
            {
                var followers = _context.Follows.Where(f => f.FollowedId == userId).ToList();
                return Ok(followers.Count);
            }
            else
            {
                return BadRequest("Invalid User Id");
            }
        }

        // count the number of users followed by a user

        [HttpGet("user/{id}")]
        public IActionResult GetFollowingsCountByUserId(string id)
        {
            if (Guid.TryParse(id, out Guid userId))
            {
                var followings = _context.Follows.Where(f => f.FollowerId == userId).ToList();
                return Ok(followings.Count);
            }
            else
            {
                return BadRequest("Invalid User Id");
            }
        }

        // follow a user
        [HttpPost]
        public IActionResult FollowUser(Guid followerId, Guid followedId)
        {
            var follow = new Models.Entities.Follow
            {
                FollowerId = followerId,
                FollowedId = followedId
            };

            _context.Follows.Add(follow);
            _context.SaveChanges();

            return Ok("User followed successfully.");
        }
    }
}
