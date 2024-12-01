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

namespace EventGoAPI.Application.Features.Query.Event.GetCreatedEvents
{
    public class GetCreatedEventsQueryHandler : IRequestHandler<GetCreatedEventsQueryRequest, GetCreatedEventsQueryResponse>
    {
        private IEventReadRepository _eventReadRepository;
        private IHttpContextAccessor _httpContextAccessor;
        public GetCreatedEventsQueryHandler(IEventReadRepository eventReadRepository, IHttpContextAccessor httpContextAccessor)
        {
            _eventReadRepository = eventReadRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<GetCreatedEventsQueryResponse> Handle(GetCreatedEventsQueryRequest request, CancellationToken cancellationToken)
        {
            if (_httpContextAccessor.HttpContext?.Items["UserId"] is not Guid userId)
            {
                return new GetCreatedEventsQueryResponse
                {
                    Success = false,
                    Message = "Invalid Id",
                    ResponseType = ResponseType.ValidationError
                };
            }

            var events = await _eventReadRepository.GetUsersCreatedEventsAsync(userId);

            return new GetCreatedEventsQueryResponse
            {
                Success = true,
                Message = "Successfully!",
                ResponseType = ResponseType.Success,
                Events = events
            };
        }
    }
}
