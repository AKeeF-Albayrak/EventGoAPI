using EventGoAPI.Application.Abstractions.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Query.Event.GetEventById
{
    public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQueryRequest, GetEventByIdQueryResponse>
    {
        private IEventReadRepository _eventReadRepository;
        public GetEventByIdQueryHandler(IEventReadRepository eventReadRepository)
        {
            _eventReadRepository = eventReadRepository;
        }
        public async Task<GetEventByIdQueryResponse> Handle(GetEventByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var _event = await _eventReadRepository.GetEntityByIdAsync(request.EventId);

            if (_event == null)
            {
                return new GetEventByIdQueryResponse
                {
                    Success = false,
                    Message = "Event Can not be Found!",
                    RepsonseType = Enums.ResponseType.NotFound,
                };
            }
            return new GetEventByIdQueryResponse
            {
                Success = true,
                Message = "Successfully",
                Event = _event,
                RepsonseType = Enums.ResponseType.Success,
            };
        }
    }
}
