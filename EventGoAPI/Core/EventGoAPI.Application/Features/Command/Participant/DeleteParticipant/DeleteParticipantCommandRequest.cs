﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.Participant.DeleteParticipant
{
    public class DeleteParticipantCommandRequest : IRequest<DeleteParticipantCommandResponse>
    {
        public Guid EventId { get; set; }
    }
}
