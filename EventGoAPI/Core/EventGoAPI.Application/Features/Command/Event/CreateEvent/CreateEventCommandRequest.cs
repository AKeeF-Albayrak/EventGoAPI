using EventGoAPI.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.Event.CreateEvent
{
    public class CreateEventCommandRequest : IRequest<CreateEventCommandResponse>
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required DateTime Date { get; set; }
        public required int Duration { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
        public required double Latitude { get; set; }
        public required double Longitude { get; set; }
        public required EventCategory Category { get; set; }
        public byte[]? Image { get; set; }
    }
}
