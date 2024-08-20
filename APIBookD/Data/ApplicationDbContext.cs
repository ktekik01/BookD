using Microsoft.EntityFrameworkCore;

namespace APIBookD.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        
    }
}