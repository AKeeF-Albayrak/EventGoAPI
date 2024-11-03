using EventGoAPI.Domain.Entities.Common;
using EventGoAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Domain.Entities
{
    public class Event : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        public Location Location { get; set; }
        public EventCategory Category { get; set; }
        public byte[] Image { get; set; }
        public User CreatedBy { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<Participant> Participants { get; set; }
    }
}
