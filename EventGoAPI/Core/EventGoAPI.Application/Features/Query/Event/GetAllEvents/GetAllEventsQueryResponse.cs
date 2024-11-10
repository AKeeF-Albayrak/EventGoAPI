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
        public int TotalEventCount { get; set; }
        public IEnumerable<Domain.Entities.Event> Events { get; set; }
    }
}
