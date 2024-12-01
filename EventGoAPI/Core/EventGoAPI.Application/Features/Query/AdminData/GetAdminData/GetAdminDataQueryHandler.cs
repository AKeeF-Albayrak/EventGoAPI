using EventGoAPI.Application.Abstractions.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Query.AdminData.GetAdminData
{
    public class GetAdminDataQueryHandler : IRequestHandler<GetAdminDataQueryRequest, GetAdminDataQueryResponse>
    {
        private IEventReadRepository _eventReadRepository;
        private IUserReadRepository _userReadRepository;
        private IFeedbackReadRepository _feedbackReadRepository;
        public GetAdminDataQueryHandler(IEventReadRepository eventReadRepository, IUserReadRepository userReadRepository, IFeedbackReadRepository feedbackReadRepository)
        {
            _eventReadRepository = eventReadRepository;
            _userReadRepository = userReadRepository;
            _feedbackReadRepository = feedbackReadRepository;
        }
        public async Task<GetAdminDataQueryResponse> Handle(GetAdminDataQueryRequest request, CancellationToken cancellationToken)
        {
            GetAdminDataQueryResponse response = new GetAdminDataQueryResponse();

            response.TotalUser = await _userReadRepository.GetUserCountAsync();
            response.UnReadedFeedbacks = await _feedbackReadRepository.GetFeedbackCountAsync();
            response.UnApprovedEventCount = await _eventReadRepository.GetUnapprovedEventCountAsync();
            response.TotalEventCount = await _eventReadRepository.GetAllEventCountAsync();
            return response;
        }
    }
}
