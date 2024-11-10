using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.Participant.CreateParticipant
{
    public class CreateParticipantCommandRequest : IRequest<CreateParticipantCommandResponse>
    {
        public Guid EventId { get; set; }
    }
}
