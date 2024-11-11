using EventGoAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Abstractions.Repositories
{
    public interface IMessageReadRepository : IReadRepository<Message>
    {
        public Task<ICollection<Message>> GetAllChatMessagesAsync(Guid eventId);
    }
}
