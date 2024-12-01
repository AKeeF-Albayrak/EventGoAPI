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

        public async Task<ICollection<Event>> GetAllEventsAsync()
        {
            return await Table
                .Include(e => e.Participants)
                .Include(e => e.Messages)
                .ToListAsync();
        }

        public async Task<ICollection<Event>> GetAllEventsForUserAsync(Guid userId)
        {
            return await _context.Events
                .Where(e => EF.Property<bool>(e, "isApproved")
                            && !e.Participants.Any(p => p.Id == userId) && e.Date > DateTime.UtcNow)
                .ToListAsync();
        }

        public async Task<int> GetAllEventCountAsync()
        {
            return await Table.CountAsync();
        }

        public async Task<List<Event>> GetUserPastEventsAsync(Guid userId)
        {
            return await _context.Participants
                .Where(p => p.Id == userId && p.Event.Date < DateTime.Now)
                .Select(p => p.Event)
                .ToListAsync();
        }

        public async Task<List<Event>> GetUsersCurrentEventsAsync(Guid userId)
        {
            return await _context.Participants
                .Where(p => p.Id == userId &&
                            p.Event.Date >= DateTime.Now &&
                            p.Event.CreatedById != userId) 
                .Select(p => p.Event)
                .ToListAsync();
        }

        public async Task<int> GetUnapprovedEventCountAsync()
        {
            return await Table.CountAsync(e => !e.isApproved);
        }

        public async Task<List<Event>> GetUsersCreatedEventsAsync(Guid userId)
        {
            return await Table
                .Where(e => e.CreatedById == userId)
                .ToListAsync();
        }
    }
}
