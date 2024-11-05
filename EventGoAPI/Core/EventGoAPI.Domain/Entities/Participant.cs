using EventGoAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EventGoAPI.Domain.Entities
{
    public class Participant : BaseEntity
    {
        public Guid EventId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
        [JsonIgnore]
        public Event? Event { get; set; }
    }
}
