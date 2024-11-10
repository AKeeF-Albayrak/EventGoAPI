using EventGoAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.Event.UpdateEvent
{
    public class UpdateEventCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
