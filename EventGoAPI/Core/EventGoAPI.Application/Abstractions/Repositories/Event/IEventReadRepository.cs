using EventGoAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Abstractions.Repositories
{
    public interface IEventReadRepository : IReadRepository<Event>
    {
        public Task<ICollection<Event>> GetAllEventsForUserAsync(Guid userId);
        public Task<ICollection<Event>> GetAllEventsAsync();
        Task<List<Event>> GetUserPastEvents(Guid userId);
        Task<List<Event>> GetUsersCurrentEvents(Guid userId);
    }
}
