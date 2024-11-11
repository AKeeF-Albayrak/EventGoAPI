using EventGoAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Abstractions.Repositories
{
    public interface IParticipantReadRepository : IReadRepository<Participant>
    {
        public Task<Participant> GetEntityByIdAsync(string id, string eventId);
        public Task<bool> HasNoParticipationAsync(Guid userId);
        Task<IEnumerable<Participant>> GetParticipantsByEventIdAsync(Guid eventId);
    }
}
