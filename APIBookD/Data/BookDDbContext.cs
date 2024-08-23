using APIBookD.Models.Entities;
using APIBookD.Models.Entities.List;
using APIBookD.Models.Entities.User;
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

        public DbSet<APIBookD.Models.Entities.User.Reviewer> Reviewers { get; set; }

        public DbSet<APIBookD.Models.Entities.User.Admin> Admins { get; set; }

        public DbSet<APIBookD.Models.Entities.Book.Book> Books { get; set; }

        public DbSet<APIBookD.Models.Entities.Chatting.Chat> Chats { get; set; }

        public DbSet<APIBookD.Models.Entities.Chatting.Message> Messages { get; set; }

        public DbSet<APIBookD.Models.Entities.Review.Review> Reviews { get; set; }

        public DbSet<APIBookD.Models.Entities.Review.CommentToReview> CommentToReviews { get; set; }

        public DbSet<APIBookD.Models.Entities.List.List> Lists { get; set; }

        public DbSet<APIBookD.Models.Entities.List.ListBook> ListBooks { get; set; }

        public DbSet<APIBookD.Models.Entities.Review.VoteReview> VoteReviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Table-per-Type (TPT) Inheritance
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Reviewer>().ToTable("Reviewers");
            modelBuilder.Entity<Admin>().ToTable("Admins");


            modelBuilder.Entity<Reviewer>()
                .HasOne<User>() // Define relationship if needed
                .WithMany() // Define collection navigation if needed
                .HasForeignKey(r => r.Id) // Adjust as per your design
                .OnDelete(DeleteBehavior.Cascade);

            // Define composite key for Follow entity
            modelBuilder.Entity<Follow>()
                .HasKey(f => new { f.FollowerId, f.FollowedId });

            // Define composite key for ListBook entity
            modelBuilder.Entity<ListBook>()
                .HasKey(lb => new { lb.ListId, lb.BookId });
        }





    }
}