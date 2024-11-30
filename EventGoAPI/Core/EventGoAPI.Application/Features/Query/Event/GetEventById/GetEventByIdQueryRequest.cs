using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Query.Event.GetEventById
{
    public class GetEventByIdQueryRequest : IRequest<GetEventByIdQueryResponse> 
    {
        public Guid EventId { get; set; }
    }
}
