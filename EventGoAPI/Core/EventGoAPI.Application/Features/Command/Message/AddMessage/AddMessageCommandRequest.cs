using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.Message.AddMessage
{
    public class AddMessageCommandRequest : IRequest<AddMessageCommandResponse>
    {
        public Guid EventId { get; set; }
        public string Message { get; set; }
    }
}
