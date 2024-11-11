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
    public class MessageReadRepository : ReadRepository<Message>, IMessageReadRepository
    {
        private readonly EventGoDbContext _context;
        public MessageReadRepository(EventGoDbContext context) : base(context)
        {
            _context = context;
        }

        public DbSet<Message> Table => _context.Set<Message>();

        public async Task<ICollection<Message>> GetAllChatMessagesAsync(Guid eventId)
        {
            return await Table
                .Where(m => m.EventId == eventId)
                .OrderBy(m => m.SendingTime)
                .ToListAsync();
        }
    }
}
