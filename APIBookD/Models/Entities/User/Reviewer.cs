namespace APIBookD.Models.Entities.User
{
    public class Reviewer: User
    {

        //profile picture
        public string ProfilePicture { get; set; }

        // bio
        public string Biography { get; set; }

        // date of birth
        public DateTime DateOfBirth { get; set; }
    }
}
