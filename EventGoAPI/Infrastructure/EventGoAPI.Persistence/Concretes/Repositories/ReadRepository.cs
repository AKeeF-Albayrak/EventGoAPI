using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Domain.Entities.Common;
using EventGoAPI.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

        public async Task<ICollection<T>> GetAllAsync() => await Table.ToListAsync();

        public async Task<T> GetEntityByIdAsync(Guid id) => await Table.FindAsync(id);
    }
}
