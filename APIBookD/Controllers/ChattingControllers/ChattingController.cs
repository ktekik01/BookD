using APIBookD.Data;
using APIBookD.Models.Entities.Chatting.ChattingDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIBookD.Controllers.ChattingControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChattingController : ControllerBase
    {
        private readonly BookDDbContext _context;

        public ChattingController(BookDDbContext context)
        {
            _context = context;
        }

        // get all chats
        [HttpGet]
        public IActionResult GetChats()
        {
            var chats = _context.Chats.ToList();
            return Ok(chats);
        }

        // get all messages in the database
        [HttpGet]
        public IActionResult GetMessages()
        {
            var messages = _context.Messages.ToList();
            return Ok(messages);
        }

        // get all messages of a chat
        [HttpGet("chat/{id}")]
        public IActionResult GetMessagesByChatId(string id)
        {
            if (Guid.TryParse(id, out Guid chatId))
            {
                var messages = _context.Messages.Where(m => m.ChatId == chatId).ToList();
                return Ok(messages);
            }
            else
            {
                return BadRequest("Invalid Chat Id");
            }
        }

        // get all messages sent by a user
        [HttpGet("user/{id}")]
        public IActionResult GetMessagesByUserId(string id)
        {
            if (Guid.TryParse(id, out Guid userId))
            {
                var messages = _context.Messages.Where(m => m.SenderId == userId).ToList();
                return Ok(messages);
            }
            else
            {
                return BadRequest("Invalid User Id");
            }
        }

        // get all messages received by a user
        [HttpGet("user/{id}")]
        public IActionResult GetMessagesReceivedByUserId(string id)
        {
            if (Guid.TryParse(id, out Guid userId))
            {
                var messages = _context.Messages.Where(m => m.ReceiverId == userId).ToList();
                return Ok(messages);
            }
            else
            {
                return BadRequest("Invalid User Id");
            }
        }


        [HttpPost]

        public IActionResult AddMessage(MessageDTO messageDTO, Guid senderId, Guid receiverId)
        {
            // check if the chat exists. iterate over all chats and check its UsersList. If the senderId is in the UsersList and the receiverId is in the UsersList, the chat exists.
            var chat = _context.Chats.FirstOrDefault(c => c.UsersList.Contains(senderId) && c.UsersList.Contains(receiverId));
            
            if (chat == null)
            {
                // create a new chat. add the senderId and the receiverId to the UsersList of the chat.
                chat = new Models.Entities.Chatting.Chat
                {
                    Id = Guid.NewGuid(),
                    UsersList = new List<Guid> { senderId, receiverId },
                    //create a message list
                    Messages = new List<Guid>()
                };

                _context.Chats.Add(chat);
                _context.SaveChanges();
            }

            // add the message to the chat. 
            var message = new Models.Entities.Chatting.Message
            {
                Id = Guid.NewGuid(),
                ChatId = chat.Id,
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = messageDTO.Content,
                Time = DateTime.Now
            };

            // the message var must be addedto the Messages list of the Chat entity.
            chat.Messages.Add(message.Id);
            _context.SaveChanges();

            _context.Messages.Add(message);
            _context.SaveChanges();

            return Ok(message);
        }

        // delete message.

    }
}
