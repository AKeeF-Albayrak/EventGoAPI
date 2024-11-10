using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Domain.Entities;
using EventGoAPI.Persistence.Concretes.Repositories;
using EventGoAPI.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Persistence.Concretes.Repositories
{
    public class EventReadRepository : ReadRepository<Event>, IEventReadRepository
    {
        private readonly EventGoDbContext _context;
        public EventReadRepository(EventGoDbContext context) : base(context)
        {
            _context = context;
        }

        public DbSet<Event> Table => _context.Set<Event>();

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await Table
                .Include(e => e.Participants)
                .Include(e => e.Messages)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetAllEventsForUserAsync() => await Table.Where(entity => EF.Property<bool>(entity, "isApproved")).ToListAsync();

        public async Task<List<Event>> GetUserPastEvents(Guid userId)
        {
            return await _context.Participants
                .Where(p => p.Id == userId && p.Event.Date < DateTime.Now)
                .Select(p => p.Event)
                .ToListAsync();
        }
    }
}
