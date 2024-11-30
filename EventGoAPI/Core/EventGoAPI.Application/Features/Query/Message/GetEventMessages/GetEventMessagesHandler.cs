using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Application.Dtos.Message;
using EventGoAPI.Application.Enums;
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
        private IUserReadRepository _userReadRepository;
        public GetEventMessagesHandler(IMessageReadRepository messageReadRepository, IEventReadRepository eventReadRepository, IParticipantReadRepository participantReadRepository, IHttpContextAccessor httpContextAccessor ,IUserReadRepository userReadRepository)
        {
            _messageReadRepository = messageReadRepository;
            _eventReadRepository = eventReadRepository;
            _participantReadRepository = participantReadRepository;
            _httpContextAccessor = httpContextAccessor;
            _userReadRepository = userReadRepository;
        }
        public async Task<GetEventMessagesResponse> Handle(GetEventMessagesRequest request, CancellationToken cancellationToken)
        {
            var _event = await _eventReadRepository.GetEntityByIdAsync(request.EventId);

            if (_event == null)
            {
                return new GetEventMessagesResponse
                {
                    Success = false,
                    Message = "Invalid Id",
                    ResponseType = ResponseType.NotFound,
                    Messages = null
                };
            }

            if (_httpContextAccessor.HttpContext?.Items["UserId"] is not Guid userId)
            {
                return new GetEventMessagesResponse
                {
                    Success = false,
                    Message = "Invalid Id",
                    ResponseType = ResponseType.ValidationError
                };
            }

            var participant = await _participantReadRepository.GetEntityByIdAsync(userId, request.EventId);

            if (participant == null)
            {
                return new GetEventMessagesResponse
                {
                    Success = false,
                    Message = "Unauthorized!",
                    ResponseType = ResponseType.Unauthorized
                };
            }

            var _messages = await _messageReadRepository.GetAllChatMessagesAsync(request.EventId);
            
            ICollection<MessageViewDto> messages = new List<MessageViewDto>();

            foreach( var message in _messages )
            {
                var user = await _userReadRepository.GetEntityByIdAsync(message.SenderId);

                messages.Add(new MessageViewDto
                {
                    Id = message.Id,
                    SenderId = message.SenderId,
                    EventId = message.EventId,
                    SendingTime = DateTime.UtcNow,
                    Text = message.Text,
                    Username = user.Username
                });
            }

            return new GetEventMessagesResponse
            {
                Success = true,
                Message = "Successfully!",
                ResponseType = ResponseType.Success,
                Messages = messages,
            };
        }
    }
}
