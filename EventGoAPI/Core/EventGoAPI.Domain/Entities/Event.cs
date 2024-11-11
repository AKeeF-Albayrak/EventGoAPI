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
        public Guid CreatedById { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public EventCategory Category { get; set; }
        public byte[]? Image { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool isApproved { get; set; }
        public User CreatedBy { get; set; }
        public List<Message>? Messages { get; set; }
        public List<Participant>? Participants { get; set; }
        public List<Point>? Points { get; set; }
    }
}
