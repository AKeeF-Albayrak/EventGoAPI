using EventGoAPI.Application.Abstractions.Repositories;
using MediatR;
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
            var events = await _eventReadRepository.GetAllAsync();

            return new()
            {
                Events = events,
                TotalEventCount = events.Count()
            };
        }
    }
}
