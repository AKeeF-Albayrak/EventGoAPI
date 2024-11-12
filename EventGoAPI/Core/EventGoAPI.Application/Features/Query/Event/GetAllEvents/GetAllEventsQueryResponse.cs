using EventGoAPI.Application.Enums;
using EventGoAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Query.Event.GetAllEvents
{
    public class GetAllEventsQueryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int TotalEventCount { get; set; }
        public IEnumerable<Domain.Entities.Event> Events { get; set; }
        public ResponseType ResponseType { get; set; }
    }
}
