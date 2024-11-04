using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Domain.Entities;
using EventGoAPI.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Persistence.Repositories
{
    public class EventWriteRepository : WriteRepository<Event>, IEventWriteRepository
    {
        private readonly EventGoDbContext _context;
        public EventWriteRepository(EventGoDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
