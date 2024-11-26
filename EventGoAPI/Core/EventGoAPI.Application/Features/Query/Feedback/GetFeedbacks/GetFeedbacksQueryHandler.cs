using EventGoAPI.Application.Abstractions.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Query.Feedback.GetFeedbacks
{
    public class GetFeedbacksQueryHandler : IRequestHandler<GetFeedbacksQueryRequest, GetFeedbacksQueryResponse>
    {
        private IFeedbackReadRepository _feedbackReadRepository;
        public GetFeedbacksQueryHandler(IFeedbackReadRepository feedbackReadRepository)
        {
            _feedbackReadRepository = feedbackReadRepository;
        }
        public async Task<GetFeedbacksQueryResponse> Handle(GetFeedbacksQueryRequest request, CancellationToken cancellationToken)
        {
            var feedbacks = await _feedbackReadRepository.GetAllAsync();
            return new GetFeedbacksQueryResponse()
            {
                Feedbacks = feedbacks,
                Success = true,
                Message = "Succesfully!",
                ResponseType = Enums.ResponseType.Success,
            };
        }
    }
}
