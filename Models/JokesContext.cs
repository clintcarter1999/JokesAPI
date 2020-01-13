using Microsoft.EntityFrameworkCore;

namespace JokesAPI.Models
{
    public class JokesContext : DbContext
    {
        public JokesContext(DbContextOptions<JokesContext> options)
            : base(options)
        {
        }

        public DbSet<JokeItem> JokeItems { get; set; }
    }
}
