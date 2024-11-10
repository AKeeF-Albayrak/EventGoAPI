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
        public Task<IEnumerable<Event>> GetAllEventsForUserAsync();
        public Task<IEnumerable<Event>> GetAllEventsAsync();
    }
}
