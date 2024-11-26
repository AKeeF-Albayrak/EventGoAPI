using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.Feedback.ReadFeedback
{
    public class ReadFeedbackCommandRequest : IRequest<ReadFeedbackCommandResponse>
    {
        public Guid FeedbackID { get; set; }
    }
}
