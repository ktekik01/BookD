using System.ComponentModel.DataAnnotations;

namespace APIBookD.Models.Entities
{
    public class Follow
    {

        // follower id
        [Key]
        public Guid FollowerId { get; set; }

        // followed id
        [Key]
        public Guid FollowedId { get; set; }
    }
}
