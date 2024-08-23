using APIBookD.Data;
using APIBookD.Models.Entities.Review;
using APIBookD.Models.Entities.User;
using APIBookD.Models.Entities.User.UserDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using APIBookD.JwtFeatures;


namespace APIBookD.Controllers.UserControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly BookDDbContext _context;
        //private readonly IEmailService _emailService;
        //private readonly JwtHandler _jwtHandler;


        public UserController(BookDDbContext context)
        {
            _context = context;
        }


        // get all users

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _context.Users.ToList();
            return Ok(users);
        }



        [HttpPost("reviewer")]
        public async Task<IActionResult> AddReviewer(ReviewerDTO _reviewer)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == _reviewer.Email);
            if (existingUser != null)
            {
                return BadRequest("The email is already in the database.");
            }

            var reviewer = new Reviewer
            {
                Id = Guid.NewGuid(),
                Name = _reviewer.Name,
                Surname = _reviewer.Surname,
                Email = _reviewer.Email,
                Password = _reviewer.Password,
                UserType = "Reviewer",
                ProfilePicture = _reviewer.ProfilePicture,
                Biography = _reviewer.Biography,
                DateOfBirth = _reviewer.DateOfBirth,
                Followers = new List<Guid>(),
                Following = new List<Guid>(),
                UpvotedReviews = new List<Guid>(),
                DownvotedReviews = new List<Guid>()
            };

            _context.Reviewers.Add(reviewer);
            await _context.SaveChangesAsync();

            return Ok(reviewer);
        }



        /*
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDTO userForAuthentication)
        {
            var user = await _
        } */



        // verify email


        /*
        [HttpGet("verify")]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            var user = _context.Users.FirstOrDefault(u => u.VerificationToken == token);

            if (user == null)
            {
                return BadRequest("Invalid token.");
            }

            user.IsVerified = true;
            user.VerificationToken = null; // Token can only be used once

            await _context.SaveChangesAsync();

            return Ok("Email verified successfully.");
        }

        */


        [HttpPost("admin")]
        public IActionResult AddAdmin(AdminDTO admin)
        {
            

            var existingUser = _context.Users.FirstOrDefault(u => u.Email == admin.Email);
            if (existingUser != null)
            {
                return BadRequest("The email is already in the database.");
            }

            var newAdmin = new Admin
            {
                Id = Guid.NewGuid(),
                Name = admin.Name,
                Surname = admin.Surname,
                Email = admin.Email,
                Password = admin.Password,
                UserType = "Admin",
                AdminRole = admin.AdminRole
            };

            _context.Admins.Add(newAdmin);
            _context.SaveChanges();


            return Ok(admin);
        }


        // get user by id
        [HttpGet("{id}")]
        public IActionResult GetUserById(Guid id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            return Ok(user);
        }

        // update reviewer. If a field is not updated, this is not an error and the old value is kept.
        [HttpPut("{id}")]
        public IActionResult UpdateReviewer(Guid id, ReviewerDTO _reviewer)
        {
            
            
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            var reviewer = _context.Reviewers.FirstOrDefault(r => r.Id == id);

            if (user == null || reviewer == null)
            {
                return NotFound();
            }

            if (_reviewer.Name != null)
            {
                user.Name = _reviewer.Name;
                reviewer.Name = _reviewer.Name;
            }

            if (_reviewer.Surname != null)
            {
                user.Surname = _reviewer.Surname;
                reviewer.Surname = _reviewer.Surname;
            }

            if (_reviewer.Email != null)
            {
                user.Email = _reviewer.Email;
                reviewer.Email = _reviewer.Email;
            }

            if (_reviewer.Password != null)
            {
                user.Password = _reviewer.Password;
                reviewer.Password = _reviewer.Password;
            }

            if (_reviewer.ProfilePicture != null)
            {
                reviewer.ProfilePicture = _reviewer.ProfilePicture;
            }

            if (_reviewer.Biography != null)
            {
                reviewer.Biography = _reviewer.Biography;
            }

            if (_reviewer.DateOfBirth != null)
            {
                reviewer.DateOfBirth = _reviewer.DateOfBirth;
            }

            _context.SaveChanges();

            return Ok(reviewer); 
        }

        // delete reviewer
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReviewer(Guid id)
        {
            var user = _context.Users.Find(id); // Use Find for primary key lookup
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user); // This will also remove related entities if cascading is configured
            _context.SaveChanges();

            return Ok();
        }



    }
}
