using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JokesAPI.Data;
using JokesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace JokesAPI.Data
{
    public class UnitOfWorkEntity<TEntity> : UnitOfWorkBase 
    {
        AppDbContext _context;
        public UnitOfWorkEntity(AppDbContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Set the State of the Entity (Modified, Saved, Deleted, etc)
        /// </summary>
        /// <param name="item">TEntity</param>
        /// <param name="state">EntityState</param>
        public void SetState(TEntity item, EntityState state)
        {
            // Implementing in this derived class.  Seem's generic enough
            _context.Entry(item).State = state;
        }
    }
}
