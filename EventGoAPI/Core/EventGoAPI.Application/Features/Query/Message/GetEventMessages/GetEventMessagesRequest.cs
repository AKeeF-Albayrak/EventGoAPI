using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Query.Message.GetEventMessages
{
    public class GetEventMessagesRequest : IRequest<GetEventMessagesResponse>
    {
        public Guid EventId { get; set; }
    }
}
