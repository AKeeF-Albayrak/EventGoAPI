using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Query.Event.GetApprovedEvents
{
    public class GetApprovedEventQueryRequest : IRequest<GetApprovedEventQueryResponse>
    {
    }
}
