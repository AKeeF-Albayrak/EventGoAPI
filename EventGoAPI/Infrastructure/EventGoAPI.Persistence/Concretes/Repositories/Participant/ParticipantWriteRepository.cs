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
    public class ParticipantWriteRepository : WriteRepository<Participant>, IParticipantWriteRepository
    {
        private readonly EventGoDbContext _context;

        public ParticipantWriteRepository(EventGoDbContext context) : base(context)
        {
            _context = context;
        }

        public DbSet<Participant> Table => _context.Set<Participant>();
    }
}
