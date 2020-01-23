using Microsoft.EntityFrameworkCore;

namespace JokesAPI.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<JokeItem> JokeItems { get; set; }

        public DbSet<JokesAPI.Models.UserInfo> UserInfo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //if (modelBuilder != null)
            //{
            //    modelBuilder.Entity<JokeItem>().HasData(
            //            new JokeItem
            //            {
            //                Id = 28,
            //                Joke = "Joke Number 28 is soooooper funny!"
            //            }
            //        );
            //}
        }
    }
}
