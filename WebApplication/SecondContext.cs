using Microsoft.EntityFrameworkCore;

namespace WebApplication
{
    public class SecondContext : DbContext
    {
        public SecondContext(DbContextOptions<SecondContext> options) : base(options)
        {
        }

        public DbSet<Blogger> Bloggers { get; set; }
    }

    public class Blogger
    {
        public string Id { get; set; }
    }
}
