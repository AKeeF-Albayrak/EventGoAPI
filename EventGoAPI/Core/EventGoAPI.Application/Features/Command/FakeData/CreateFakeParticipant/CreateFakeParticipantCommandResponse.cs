using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.FakeData.CreateFakeParticipant
{
    public class CreateFakeParticipantCommandResponse
    {
        public bool Success { get; set; }   
        public Domain.Entities.Participant Participant { get; set; }
    }
}
