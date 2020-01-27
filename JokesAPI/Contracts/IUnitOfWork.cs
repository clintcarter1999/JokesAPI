using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JokesAPI.Contracts
{
    public interface IUnitOfWork
    {
        IJokeItemRepository JokeItems { get; }

        int Complete(); // could call this Save.  We are "completing" a Unit of Work
    }
}
