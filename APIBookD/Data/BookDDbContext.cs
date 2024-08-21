using Microsoft.EntityFrameworkCore;

namespace APIBookD.Data
{
    public class BookDDbContext : DbContext
    {
        public BookDDbContext(DbContextOptions<BookDDbContext> options) : base(options)
        {
        }

        // DbSets

        public DbSet<APIBookD.Models.Entities.Follow> Follows { get; set; }

        public DbSet<APIBookD.Models.Entities.User.User> Users  { get; set; }

        public DbSet<APIBookD.Models.Entities.User.Admin> Admins { get; set; }

        public DbSet<APIBookD.Models.Entities.User.Reviewer> Reviewers { get; set; }

        public DbSet<APIBookD.Models.Entities.Book.Book> Books { get; set; }

        public DbSet<APIBookD.Models.Entities.Chatting.Chat> Chats { get; set; }

        public DbSet<APIBookD.Models.Entities.Chatting.Message> Messages { get; set; }

        public DbSet<APIBookD.Models.Entities.Review.Review> Reviews { get; set; }

        public DbSet<APIBookD.Models.Entities.Review.CommentToReview> CommentToReviews { get; set; }

        public DbSet<APIBookD.Models.Entities.List.List> Lists { get; set; }

        public DbSet<APIBookD.Models.Entities.List.ListBook> ListBooks { get; set; }




        
    }
}