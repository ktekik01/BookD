using APIBookD.Data;
using APIBookD.Models.Entities.Chatting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIBookD.Models.Entities.Chatting.ChattingDTOs;


namespace APIBookD.Controllers.ChattingControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChattingController : ControllerBase
    {
        private readonly BookDDbContext _context;
        private readonly IHubContext<ChatHub> _chatHubContext;

        public ChattingController(BookDDbContext context, IHubContext<ChatHub> chatHubContext)
        {
            _context = context;
            _chatHubContext = chatHubContext;
        }

        // get all chats
        [HttpGet("GetChats")]
        public IActionResult GetChats()
        {
            var chats = _context.Chats.ToList();
            return Ok(chats);
        }

        // get chat by user id
        [HttpGet("GetChats/{id}")]
        public IActionResult GetChatsByUserId(string id)
        {
            if (Guid.TryParse(id, out Guid userId))
            {
                var chats = _context.Chats.Where(c => c.UsersList.Contains(userId)).ToList();
                return Ok(chats);
            }
            else
            {
                return BadRequest("Invalid User Id");
            }
        }

        // get all messages in the database
        [HttpGet("GetMessages")]
        public IActionResult GetMessages()
        {
            var messages = _context.Messages.ToList();
            return Ok(messages);
        }

        // get all messages of a chat. Get the oldest message first. newest message last.

        [HttpGet("chat/{id}/messages")]
        public IActionResult GetMessagesByChatId(string Id)
        {
            if (Guid.TryParse(Id, out Guid chatId))
            {
                // get the oldest message first, newest message last. use Time column for sorting.

                var messages = _context.Messages.Where(m => m.ChatId == chatId).OrderBy(m => m.Time).ToList();

                //var messages = _context.Messages.Where(m => m.ChatId == chatId).ToList();
                return Ok(messages);
            }
            else
            {
                return BadRequest("Invalid Chat Id");
            }
        }


        // get all messages sent by a user
        [HttpGet("user/sent/{id}")]
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
        [HttpGet("user/received/{id}")]
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


        [HttpPost("AddMessage")]
        public async Task<IActionResult> AddMessage([FromBody] MessageRequest request)
        {
            var chat = _context.Chats.FirstOrDefault(c => c.UsersList.Contains(request.SenderId) && c.UsersList.Contains(request.ReceiverId));

            if (chat == null)
            {
                chat = new Chat
                {
                    Id = Guid.NewGuid(),
                    UsersList = new List<Guid> { request.SenderId, request.ReceiverId },
                    Messages = new List<Guid>()
                };

                await _context.Chats.AddAsync(chat);
                await _context.SaveChangesAsync();
            }

            var message = new Message
            {
                Id = Guid.NewGuid(),
                ChatId = chat.Id,
                SenderId = request.SenderId,
                ReceiverId = request.ReceiverId,
                Content = request.MessageDTO.Content,
                Time = DateTime.Now
            };

            chat.Messages.Add(message.Id);
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();

            await _chatHubContext.Clients.All.SendAsync("ReceiveMessage", request.SenderId.ToString(), message.Content, request.ReceiverId.ToString());


            // Return the message object to be used by the SignalR hub
            return Ok(message);
        }

        // DTO for request body
        public class MessageRequest
        {
            public Guid SenderId { get; set; }
            public Guid ReceiverId { get; set; }
            public MessageDTO MessageDTO { get; set; }
        }


        // delete message. First, remove the message from the Messages list of the Chat entity. Then, remove the message from the Messages table.

        [HttpDelete("{id}")]
        public IActionResult DeleteMessage(string id)
        {
            if (Guid.TryParse(id, out Guid messageId))
            {
                var message = _context.Messages.FirstOrDefault(m => m.Id == messageId);
                if (message != null)
                {
                    var chat = _context.Chats.FirstOrDefault(c => c.Id == message.ChatId);
                    chat.Messages.Remove(message.Id);
                    _context.Messages.Remove(message);
                    _context.SaveChanges();
                    return Ok("Message deleted");
                }
                else
                {
                    return NotFound("Message not found");
                }
            }
            else
            {
                return BadRequest("Invalid Message Id");
            }
        }

        // delete chat. First, remove all messages of the chat from the Messages table. Then, remove the chat from the Chats table.

        [HttpDelete("chat/{id}")]
        public IActionResult DeleteChat(string id)
        {
            if (Guid.TryParse(id, out Guid chatId))
            {
                var chat = _context.Chats.FirstOrDefault(c => c.Id == chatId);
                if (chat != null)
                {
                    foreach (var messageId in chat.Messages)
                    {
                        var message = _context.Messages.FirstOrDefault(m => m.Id == messageId);
                        _context.Messages.Remove(message);
                    }
                    _context.Chats.Remove(chat);
                    _context.SaveChanges();
                    return Ok("Chat deleted");
                }
                else
                {
                    return NotFound("Chat not found");
                }
            }
            else
            {
                return BadRequest("Invalid Chat Id");
            }
        }



    }
}
