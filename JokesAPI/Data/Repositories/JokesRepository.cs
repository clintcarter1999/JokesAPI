using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JokesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace JokesAPI.Persistence
{
    public class JokesRepository : Repository<JokeItem>, IJokesRepository
    {
        public JokesRepository(AppDbContext context) : base(context)
        {
            // right now just passing context through to base.
            // TODO: Add specific methods for the JokesController here.
            // Paging, Random, etc..
        }

        public async Task<IEnumerable<JokeItem>> GetJokesPage(int pageIndex, int pageSize = 10)
        {
            return await DataBaseContext.JokeItems.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<IEnumerable<JokeItem>> Contains(string text)
        {
            //TODO: Add an extension class for CaseInsensitive.  See:
            // https://dotnetthoughts.net/how-to-make-string-contains-case-insensitive/

            return await DataBaseContext.JokeItems.Where(j => j.Joke.Contains(text)).ToListAsync();
        }
    }
}
