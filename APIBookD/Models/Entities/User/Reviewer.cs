namespace APIBookD.Models.Entities.User
{
    public class Reviewer: User
    {

        // profile picture
        public string? ProfilePicture { get; set; }

        // bio
        public string? Biography { get; set; }

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




    }
}
