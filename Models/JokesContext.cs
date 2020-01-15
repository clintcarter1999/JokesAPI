using Microsoft.EntityFrameworkCore;
using JokesAPI.Models;

namespace JokesAPI.Models
{
    public class JokesContext : DbContext
    {
        public JokesContext(DbContextOptions<JokesContext> options)
            : base(options)
        {
        }

        public DbSet<JokeItem> JokeItems { get; set; }

        public DbSet<JokesAPI.Models.User> User { get; set; }
    }
}
