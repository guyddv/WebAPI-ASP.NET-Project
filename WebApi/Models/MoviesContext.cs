using Microsoft.EntityFrameworkCore;

namespace WebApi.Models
{
    public class MoviesContext : DbContext
    {
        public MoviesContext(DbContextOptions<MoviesContext> options)
               : base(options)
        {
        }

        public DbSet<Movies> Movie { get; set; }
    }
}