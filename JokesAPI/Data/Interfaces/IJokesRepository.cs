using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JokesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace JokesAPI.Persistence
{
    public interface IJokesRepository : IRepository<JokeItem>
    {
        //TODO: Add the other interfaces after getting the get working
        int Count { get; }

        Task<IEnumerable<JokeItem>> GetJokesPage(int pageIndex, int pageSize);

        Task<IEnumerable<JokeItem>> Contains(string text);

    }
}
