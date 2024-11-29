using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Application.Enums;
using EventGoAPI.Application.Features.Command.Event.CreateEvent;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Query.Event.GetCurrentEvents
{
    public class GetCurrentEventsQueryHandler : IRequestHandler<GetCurrentEventsQueryRequest, GetCurrentEventsQueryResponse>
    {
        private IEventReadRepository _eventReadRepository;
        private IHttpContextAccessor _httpContextAccessor;
        public GetCurrentEventsQueryHandler(IEventReadRepository eventReadRepository, IHttpContextAccessor httpContextAccessor)
        {
            _eventReadRepository = eventReadRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetCurrentEventsQueryResponse> Handle(GetCurrentEventsQueryRequest request, CancellationToken cancellationToken)
        {
            if (_httpContextAccessor.HttpContext?.Items["UserId"] is not Guid userId)
            {
                return new GetCurrentEventsQueryResponse
                {
                    Success = false,
                    Message = "Invalid Id",
                    ResponseType = ResponseType.ValidationError
                };
            }

            var events = await _eventReadRepository.GetUserPastEventsAsync(userId);

            return new GetCurrentEventsQueryResponse
            {
                Success = true,
                Message = "Successfully!",
                ResponseType = ResponseType.Success,
                Events = events
            };
        }
    }
}
