using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Domain.Entities;
using EventGoAPI.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Persistence.Concretes.Repositories
{
    public class PointReadRepository : ReadRepository<Point>, IPointReadRepository
    {
        private readonly EventGoDbContext _context;
        public PointReadRepository(EventGoDbContext context) : base(context)
        {
            _context = context;
        }

        public DbSet<Domain.Entities.Point> Table => _context.Set<Domain.Entities.Point>();

        public async Task<ICollection<Point>> GetPointsAsync(Guid userID)
        {
            return await Table
                .Where(point => point.UserId == userID)
                .ToListAsync();
        }

    }
}
