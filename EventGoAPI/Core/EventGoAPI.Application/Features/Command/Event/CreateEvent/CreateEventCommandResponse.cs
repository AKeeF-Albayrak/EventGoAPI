using EventGoAPI.Application.Enums;
using EventGoAPI.Domain.Entities;
using EventGoAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.Event.CreateEvent
{
    public class CreateEventCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ResponseType ResponseType { get; set; }
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
        //public byte[]? Image { get; set; }
        public DateTime? CreatedTime { get; set; }
        public bool? isApproved { get; set; }
        public int? Point { get; set; }  
    }
}
