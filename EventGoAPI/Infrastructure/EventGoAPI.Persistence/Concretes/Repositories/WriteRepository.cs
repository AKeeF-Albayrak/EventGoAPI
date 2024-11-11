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
    public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
    {
        private readonly EventGoDbContext _context;
        public WriteRepository(EventGoDbContext context)
        {
            _context = context;
        }
        public DbSet<T> Table => _context.Set<T>();

        public async Task AddAsync(T entity) => await Table.AddAsync(entity);

        public async Task DeleteAsync(Guid id)
        {
            var entity = await Table.FirstOrDefaultAsync(e => e.Id == id);

            if (entity == null)
            {
                throw new KeyNotFoundException("Entity with the specified ID was not found.");
            }

            Table.Remove(entity);
        }

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task<T> UpdateAsync(T entity)
        {
            var existingEntity = await Table.FirstOrDefaultAsync(e => e.Id == entity.Id);

            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            return existingEntity;
        }
    }
}
