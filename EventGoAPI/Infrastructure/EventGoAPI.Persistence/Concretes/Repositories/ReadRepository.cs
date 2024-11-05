using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Domain.Entities.Common;
using EventGoAPI.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Persistence.Concretes.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {
        private readonly EventGoDbContext _context;

        public ReadRepository(EventGoDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();

        public async Task<T> GetEntityByIdAsync(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
            {
                throw new ArgumentException("Invalid ID format", nameof(id));
            }

            return await Table.FindAsync(guidId);
        }
    }
}
