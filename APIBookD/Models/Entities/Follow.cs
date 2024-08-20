namespace APIBookD.Models.Entities
{
    public class Follow
    {

        // follower id
        public Guid FollowerId { get; set; }

        // followed id
        public Guid FollowedId { get; set; }

    }
}
