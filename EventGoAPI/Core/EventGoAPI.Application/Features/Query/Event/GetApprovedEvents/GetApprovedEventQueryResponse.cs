using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Query.Event.GetApprovedEvents
{
    public class GetApprovedEventQueryResponse
    {
        public IEnumerable<Domain.Entities.Event> Events { get; set; }
    }
}
