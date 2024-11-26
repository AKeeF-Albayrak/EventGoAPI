using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Query.Feedback.GetFeedbacks
{
    public class GetFeedbacksQueryRequest : IRequest<GetFeedbacksQueryResponse>
    {
    }
}
