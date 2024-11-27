using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.FakeData.CreateFakeParticipant
{
    public class CreateFakeParticipantCommandRequest : IRequest<CreateFakeParticipantCommandResponse>
    {
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
    }
}
