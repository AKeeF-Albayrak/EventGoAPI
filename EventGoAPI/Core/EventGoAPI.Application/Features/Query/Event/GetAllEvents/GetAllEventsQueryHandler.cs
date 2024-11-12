using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Application.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Query.Event.GetAllEvents
{
    public class GetAllEventsQueryHandler : IRequestHandler<GetAllEventsQueryRequest, GetAllEventsQueryResponse>
    {
        private IEventReadRepository _eventReadRepository;

        public GetAllEventsQueryHandler(IEventReadRepository eventReadRepository)
        {
            _eventReadRepository = eventReadRepository;
        }
        public async Task<GetAllEventsQueryResponse> Handle(GetAllEventsQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var events = await _eventReadRepository.GetAllEventsAsync();

                if (events == null || !events.Any())
                {
                    return new GetAllEventsQueryResponse
                    {
                        Success = false,
                        Message = "No events found.",
                        ResponseType = ResponseType.NotFound,
                        Events = Enumerable.Empty<Domain.Entities.Event>(),
                        TotalEventCount = 0
                    };
                }

                return new GetAllEventsQueryResponse
                {
                    Success = true,
                    Message = "Events retrieved successfully.",
                    ResponseType = ResponseType.Success,
                    Events = events,
                    TotalEventCount = events.Count()
                };
            }
            catch (Exception ex)
            {
                return new GetAllEventsQueryResponse
                {
                    Success = false,
                    Message = "An error occurred while retrieving events.",
                    ResponseType = ResponseType.ServerError,
                    Events = Enumerable.Empty<Domain.Entities.Event>(),
                    TotalEventCount = 0
                };
            }
        }
    }
}
