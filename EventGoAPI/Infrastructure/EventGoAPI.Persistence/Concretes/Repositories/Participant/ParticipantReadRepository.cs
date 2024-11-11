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
    public class ParticipantReadRepository : ReadRepository<Participant>, IParticipantReadRepository
    {
        private readonly EventGoDbContext _context;

        public ParticipantReadRepository(EventGoDbContext context) : base(context)
        {
            _context = context;
        }

        public DbSet<Participant> Table => _context.Set<Participant>();

        public async Task<Participant> GetEntityByIdAsync(string id, string eventId)
        {
            if (!Guid.TryParse(id, out Guid guidId) || !Guid.TryParse(eventId, out Guid guidEventId))
            {
                throw new ArgumentException("Invalid ID format");
            }

            return await Table.FindAsync(guidId, guidEventId);
        }

        public async Task<bool> HasNoParticipationAsync(Guid userId)
        {
            return await Table
                .Where(p => p.Id == userId && !_context.Events.Any(e => e.Id == p.EventId && e.CreatedById == userId))
                .AnyAsync() == false;
        }
    }
}
