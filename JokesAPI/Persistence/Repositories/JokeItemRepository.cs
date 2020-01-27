using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JokesAPI.Contracts;
using JokesAPI.Models;

namespace JokesAPI.Persistence
{
    public class JokeItemRepository : Repository<JokeItem>, IJokeItemRepository
    {
        public JokeItemRepository(AppDbContext context) : base(context)
        {
            // right now just passing context through to base.
            // TODO: Add specific methods for the JokesController here.
            // Paging, Random, etc..
        }

        public AppDbContext DataBaseContext
        {
            get { return Context as AppDbContext; }
        }

    }
}
