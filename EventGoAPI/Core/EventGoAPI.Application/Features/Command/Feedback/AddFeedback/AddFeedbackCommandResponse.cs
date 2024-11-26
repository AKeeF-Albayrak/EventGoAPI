using EventGoAPI.Application.Enums;
using EventGoAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.Feedback.AddFeedback
{
    public class AddFeedbackCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ResponseType ResponseType { get; set; }
        public Domain.Entities.Feedback Feedback { get; set; }
    }
}
