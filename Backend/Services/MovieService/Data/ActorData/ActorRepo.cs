using Microsoft.EntityFrameworkCore;
using MovieService.Models;
using System;

namespace MovieService.Data.ActorData
{
    public class ActorRepo : IActorRepo
    {
        private readonly MovieContext _context;

        public ActorRepo(MovieContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Actor>> GetAllAsync()
        {
            return await _context.Actors.ToListAsync();
        }

        public async Task<Actor> GetByIdAsync(int id)
        {
            return await _context.Actors.FindAsync(id);
        }

        public async Task InsertAsync(Actor actor)
        {
            await _context.Actors.AddAsync(actor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Actor actor)
        {
            _context.Actors.Update(actor);
            await _context.SaveChangesAsync();
        }

        public async Task HiddentActorAsync(int id)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor != null)
            {
                actor.Status = "Inactive";
                _context.Actors.Update(actor);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> CountAsync()
        {
            return await _context.Actors.CountAsync();
        }
    }
}
