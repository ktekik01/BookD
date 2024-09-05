namespace APIBookD.Models.Entities.AskToAi
{
    public class Answer
    {

        public Guid Id { get; set; }


        public Guid UserId { get; set; }

        public string AnswerText { get; set; }

        public Guid QuestionId { get; set; }

    }
}
