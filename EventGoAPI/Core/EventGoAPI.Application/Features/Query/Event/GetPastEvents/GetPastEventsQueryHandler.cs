using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Application.Enums;
using EventGoAPI.Application.Features.Query.Event.GetCurrentEvents;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Query.Event.GetPastEvents
{
    public class GetPastEventsQueryHandler : IRequestHandler<GetPastEventsQueryRequest, GetPastEventsQueryResponse>
    {
        private IEventReadRepository _eventReadRepository;
        private IHttpContextAccessor _httpContextAccessor;
        public GetPastEventsQueryHandler(IEventReadRepository eventReadRepository, IHttpContextAccessor httpContextAccessor)
        {
            _eventReadRepository = eventReadRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<GetPastEventsQueryResponse> Handle(GetPastEventsQueryRequest request, CancellationToken cancellationToken)
        {
            if (_httpContextAccessor.HttpContext?.Items["UserId"] is not Guid userId)
            {
                return new GetPastEventsQueryResponse
                {
                    Success = false,
                    Message = "Invalid Id",
                    ResponseType = ResponseType.ValidationError
                };
            }

            var events = await _eventReadRepository.GetUserPastEventsAsync(userId);

            return new GetPastEventsQueryResponse
            {
                Success = true,
                Message = "Successfully!",
                ResponseType = ResponseType.Success,
                Events = events
            };
        }
    }
}
