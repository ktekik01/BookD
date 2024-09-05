using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using APIBookD.Models.Entities.Request;
using APIBookD.Data;

namespace APIBookD.Controllers.CustomerSupportControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerSupportController : ControllerBase
    {

        private readonly BookDDbContext _context;

        public CustomerSupportController(BookDDbContext context)
        {
            _context = context;
        }

        // a method to get all requests
        [HttpGet("GetRequests")]
        public IActionResult GetRequests()
        {
            var requests = _context.Requests.ToList();
            return Ok(requests);
        }

        // a method to get a request by id
        [HttpGet("GetRequest/{id}")]
        public IActionResult GetRequest(string id)
        {
            var request = _context.Requests.Find(Guid.Parse(id));
            return Ok();
        }


        public class RequestDto
        {
            public Guid UserId { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
        }


        [HttpPost("CreateRequest")]
        public IActionResult CreateRequest([FromBody] RequestDto requestDto)
        {
            if (requestDto == null || string.IsNullOrEmpty(requestDto.Title) || string.IsNullOrEmpty(requestDto.Content))
            {
                return BadRequest("Title and content are required.");
            }

            var request = new Request
            {
                UserId = requestDto.UserId,
                Title = requestDto.Title,
                Content = requestDto.Content,
                RequestDate = DateTime.Now,
                Status = "Pending"
            };

            // Save the request to the database
            _context.Requests.Add(request);
            _context.SaveChanges();

            return Ok();
        }

    }
}
