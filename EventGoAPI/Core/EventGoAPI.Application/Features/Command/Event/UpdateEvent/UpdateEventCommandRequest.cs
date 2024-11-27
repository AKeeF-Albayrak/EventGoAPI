using EventGoAPI.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.Event.UpdateEvent
{
    public class UpdateEventCommandRequest : IRequest<UpdateEventCommandResponse>
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? Date { get; set; }
        public int? Duration { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public EventCategory? Category { get; set; }
        public byte[]? Image { get; set; }
    }
}
