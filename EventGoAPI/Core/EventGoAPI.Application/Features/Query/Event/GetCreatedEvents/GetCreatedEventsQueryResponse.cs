using EventGoAPI.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Query.Event.GetCreatedEvents
{
    public class GetCreatedEventsQueryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ResponseType ResponseType { get; set; }
        public List<Domain.Entities.Event> Events { get; set; }
    }
}
