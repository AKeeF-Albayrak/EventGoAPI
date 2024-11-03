using EventGoAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Domain.Entities
{
    public class Participant : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        public User User { get; set; }
        public Event Event { get; set; }
    }
}
