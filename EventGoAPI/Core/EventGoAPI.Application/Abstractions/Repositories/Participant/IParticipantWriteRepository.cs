using EventGoAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Abstractions.Repositories
{
    public interface IParticipantWriteRepository : IWriteRepository<Participant>
    {
        public Task DeleteAsync(string id, string eventId);
    }
}
