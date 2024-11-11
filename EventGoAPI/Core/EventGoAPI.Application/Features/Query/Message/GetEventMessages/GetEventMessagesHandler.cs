using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Application.Features.Command.Event.CreateEvent;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Query.Message.GetEventMessages
{
    public class GetEventMessagesHandler : IRequestHandler<GetEventMessagesRequest, GetEventMessagesResponse>
    {
        private IMessageReadRepository _messageReadRepository;
        private IEventReadRepository _eventReadRepository;
        private IParticipantReadRepository _participantReadRepository;
        private IHttpContextAccessor _httpContextAccessor;
        public GetEventMessagesHandler(IMessageReadRepository messageReadRepository, IEventReadRepository eventReadRepository, IParticipantReadRepository participantReadRepository, IHttpContextAccessor httpContextAccessor)
        {
            _messageReadRepository = messageReadRepository;
            _eventReadRepository = eventReadRepository;
            _participantReadRepository = participantReadRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<GetEventMessagesResponse> Handle(GetEventMessagesRequest request, CancellationToken cancellationToken)
        {
            var _event = await _eventReadRepository.GetEntityByIdAsync(request.EventId);

            if (_event == null)
            {
                return new GetEventMessagesResponse{
                    Success = false,
                    Message = "Invalid Id",
                    Messages = null
                };
            }
            if (_httpContextAccessor.HttpContext?.Items["UserId"] is not Guid userId)
            {
                return new GetEventMessagesResponse
                {
                    Success = false,
                    Message = "Invalid Id"
                };
            }
            var participant = await _participantReadRepository.GetEntityByIdAsync( userId, request.EventId);

            if (participant == null)
            {
                return new GetEventMessagesResponse
                {
                    Success = false,
                    Message = "Unauthorized!"
                };
            }

            var messages = await _messageReadRepository.GetAllChatMessagesAsync(request.EventId);

            return new GetEventMessagesResponse
            {
                Success = true,
                Message = "Successfully!",
                Messages = messages
            };
        }
    }
}
