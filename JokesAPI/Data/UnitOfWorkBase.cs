using System;
using System.Threading.Tasks;
using JokesAPI.Data.Interfaces;
using JokesAPI.Data.Repositories;
using JokesAPI.Models;

namespace JokesAPI.Data
{
    public class UnitOfWorkBase : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _context;
        public IJokesRepository JokeItems { get; private set; }
        
        // Note: You can add other IRepository derived interfaces here.
        // This will allow building of complex relationships between Models/Domains

        public UnitOfWorkBase(AppDbContext context)
        {
            _context = context;
            JokeItems = new JokesRepository(_context);

        }

        public async Task<int> Complete()
        {
            //Once I add other IRepository derived Interfaces we can implement one save here
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                   // _context = null;
                }
            }
                   
        }
    }
}
