using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.FakeData.CreateFakeMessage
{
    public class CreateFakeMessageCommandRequest : IRequest<CreateFakeMessageCommandResponse>
    {
        public Guid SenderId { get; set; }
        public Guid EventId { get; set; }
        public DateTime SendingTime { get; set; }
        public string Message { get; set; }
    }
}
