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
    public class NotificationReadRepository : ReadRepository<Notification>, INotificationReadRepository
    {
        private readonly EventGoDbContext _context;
        public NotificationReadRepository(EventGoDbContext context) : base(context)
        {
            _context = context;
        }

        public DbSet<Notification> Table => _context.Set<Notification>();

        public async Task<List<Notification>> GetNotificationsAsync(Guid userid) => await Table.Where(notification => notification.UserId == userid).ToListAsync();
    }
}
