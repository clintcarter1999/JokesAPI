using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JokesAPI.Models;

namespace JokesAPI.Contracts
{
    public interface IJokeItemRepository : IRepository<JokeItem>
    {
        //TODO: Add the other interfaces after getting the get working
        int Count { get; }

        Task<IEnumerable<JokeItem>> GetJokesPage(int pageIndex, int pageSize);

        Task<IEnumerable<JokeItem>> Contains(string text);
    }
}
