using JokesAPI.Contracts;
using JokesAPI.Models;

namespace JokesAPI.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IJokeItemRepository JokeItems { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            JokeItems = new JokeItemRepository(_context);

        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
