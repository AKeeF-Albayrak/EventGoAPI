using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Domain.Entities;
using EventGoAPI.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Persistence.Repositories
{
    public class EventReadRepository : ReadRepository<Event>, IEventReadRepository
    {
        private readonly EventGoDbContext _context;
        public EventReadRepository(EventGoDbContext context) : base(context)
        {
            _context = context;
        }

        public DbSet<Event> Table => _context.Set<Event>();
    }
}
