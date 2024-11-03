using EventGoAPI.Domain.Entities.Common;
using EventGoAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Domain.Entities
{
    public class User : BaseEntity
    {
        public Guid Id { get; set; }
        public UserRole Role { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public ICollection<EventCategory> Interests { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public bool Gender { get; set; }
        public string PhoneNumber { get; set; }
        public byte[] Image { get; set; }

        public bool IsOnline { get; set; }
        public DateTime LastActive { get; set; } = DateTime.Now;

        public ICollection<Point> Points { get; set; }
        public ICollection<Event> Events { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<Participant> Participants { get; set; }
    }
}
