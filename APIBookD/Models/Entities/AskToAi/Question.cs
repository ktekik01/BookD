namespace APIBookD.Models.Entities.AskToAi
{
    public class Question
    {
            // question id
        public Guid Id { get; set; }

        // question text
        public string QuestionText { get; set; }

        // user id
        public Guid UserId { get; set; }

        // answer id
        public Guid? AnswerId { get; set; }
    }
}
