using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.Event.DeleteEvent
{
    public class DeleteEventCommandResponse
    {
        public bool Success { get; set; }
        public int DeletedParticipant {  get; set; }
    }
}
