namespace APIBookD.Models.Entities.User
{
    public class User
    {

        public Guid Id { get; set; }
        public string Name { get; set; }

        //surname
        public string Surname { get; set; }
        public string Email { get; set; }

        //profile picture
        public string ProfilePicture { get; set; }

        // bio
        public string Biography { get; set; }

        // date of birth
        public DateTime DateOfBirth { get; set; }

        // followers list

        public List<Guid>? Followers { get; set; }

        // following list

        public List<Guid>? Following { get; set; }

        // upvoted reviews

        public List<Guid>? UpvotedReviews { get; set; }

        // downvoted reviews

        public List<Guid>? DownvotedReviews { get; set; }

        public string Password { get; set; }

        // user type
        public string UserType { get; set; }

        // verification token

        // is verified








    }
}
