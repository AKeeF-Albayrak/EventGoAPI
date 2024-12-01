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
    public class FeedbackReadRepository : ReadRepository<Feedback>, IFeedbackReadRepository
    {
        private readonly EventGoDbContext _context;
        public FeedbackReadRepository(EventGoDbContext context) : base(context)
        {
            _context = context;
        }

        public DbSet<Feedback> Table => _context.Set<Feedback>();

        public async Task<int> GetFeedbackCountAsync()
        {
            return await Table.CountAsync(f => !f.IsRead);
        }
    }
}
