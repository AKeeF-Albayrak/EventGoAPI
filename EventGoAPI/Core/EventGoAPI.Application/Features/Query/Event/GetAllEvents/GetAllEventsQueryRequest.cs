using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace EventGoAPI.Application.Features.Query.Event.GetAllEvents
{
    public class GetAllEventsQueryRequest : IRequest<GetAllEventsQueryResponse>
    {
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 5;
    }
}
