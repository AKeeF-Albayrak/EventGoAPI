using EventGoAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Dtos.Message
{
    public class MessageViewDto
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid EventId { get; set; }
        public string Username { get; set; }
        public string Text { get; set; }
        public DateTime SendingTime { get; set; }
        public Event Event { get; set; }
        public User Sender { get; set; }
    }
}
