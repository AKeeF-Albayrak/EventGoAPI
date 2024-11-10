using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.Event.ApproveEvent
{
    public class ApproveEventCommandRequest : IRequest<ApproveEventCommandResponse>
    {
        public Guid Id { get; set; }
    }
}
