using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.Event.DeleteEvent
{
    public class DeleteEventCommandRequest : IRequest<DeleteEventCommandResponse>
    {
        public Guid Id { get; set; }
    }
}
