using EventGoAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Domain.Entities
{
    public class Message : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid EventId { get; set; }
        public string Text { get; set; }
        public DateTime SendingTime { get; set; }

        public User Sender { get; set; }
        public Event Event { get; set; }
    }
}
