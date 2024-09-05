using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GroqSharp;
using APIBookD.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using APIBookD.Models.Entities.AskToAi;
using System;
using APIBookD.Data;

namespace APIBookD.Controllers.AskToAiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AskToAiController : ControllerBase
    {

        private readonly BookDDbContext _context;
        private readonly ILogger<AskToAiController> _logger;
        private IGroqClient _groqClient;


        public AskToAiController(ILogger<AskToAiController> logger, IGroqClient groqClient, BookDDbContext context)
        {
            _logger = logger;
            _groqClient = groqClient;
            _context = context;
        }


        public class QuestionDto
        {
            public Guid UserId { get; set; }

            public string QuestionText { get; set; }

        }



            
        [HttpPost("Ask")]
        public async Task<IActionResult> Ask(QuestionDto questionDto)
        {
            var question = new Question 
            {
                Id = Guid.NewGuid(),
                UserId = questionDto.UserId,
                QuestionText = questionDto.QuestionText,
            };


            // send the question to the AI

            string answer = await _groqClient.CreateChatCompletionAsync(new GroqSharp.Models.Message { Content = questionDto.QuestionText });

            // save the answer to the database

            var answerEntity = new Answer
            {
                Id = Guid.NewGuid(),
                QuestionId = question.Id,
                AnswerText = answer,
                UserId = question.UserId

            };

            question.AnswerId = answerEntity.Id;

            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            _context.Answers.Add(answerEntity);
            await _context.SaveChangesAsync();

            return Ok(new { question = questionDto.QuestionText, answer });

        }
    }
}

