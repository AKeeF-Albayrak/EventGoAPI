using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.Feedback.AddFeedback
{
    public class AddFeedbackCommandRequest : IRequest<AddFeedbackCommandResponse>
    {
        public string Message { get; set; } 
    }
}
