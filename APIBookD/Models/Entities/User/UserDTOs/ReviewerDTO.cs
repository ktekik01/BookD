namespace APIBookD.Models.Entities.User.UserDTOs
{
    public class ReviewerDTO
    {

        public string Name { get; set; }

        //surname
        public string Surname { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }

        //profile picture
        public string? ProfilePicture { get; set; }

        // bio
        public string Biography { get; set; }

        // date of birth
        public DateTime DateOfBirth { get; set; }
    }
}
