using Microsoft.EntityFrameworkCore;

namespace WebApplication
{
    public class SchemaContext : DbContext
    {
        public SchemaContext(DbContextOptions<SchemaContext> options) :
            base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Blogger> Bloggers { get; set; }
    }
}
