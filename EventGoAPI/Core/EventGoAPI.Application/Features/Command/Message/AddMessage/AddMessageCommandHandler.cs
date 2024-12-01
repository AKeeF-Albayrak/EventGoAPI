using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Application.Abstractions.Services;
using EventGoAPI.Application.Enums;
using EventGoAPI.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.Message.AddMessage
{
    public class AddMessageCommandHandler : IRequestHandler<AddMessageCommandRequest, AddMessageCommandResponse>
    {
        private IMessageWriteRepository _messageWriteRepository;
        private IParticipantReadRepository _participantReadRepository;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly INotificationService _notificationService;
        private IEventReadRepository _eventReadRepository;
        private INotificationWriteRepository _notificationWriteRepository;
        public AddMessageCommandHandler(IMessageWriteRepository messageWriteRepository, IHttpContextAccessor httpContextAccessor, IParticipantReadRepository participantReadRepository, INotificationService notificationService, IEventReadRepository eventReadRepository, INotificationWriteRepository notificationWriteRepository)
        {
            _messageWriteRepository = messageWriteRepository;
            _httpContextAccessor = httpContextAccessor;
            _participantReadRepository = participantReadRepository;
            _notificationService = notificationService;
            _eventReadRepository = eventReadRepository;
            _notificationWriteRepository = notificationWriteRepository;
        }
        public async Task<AddMessageCommandResponse> Handle(AddMessageCommandRequest request, CancellationToken cancellationToken)
        {
            if (_httpContextAccessor.HttpContext?.Items["UserId"] is not Guid userId)
            {
                return new AddMessageCommandResponse
                {
                    Success = false,
                    Message = "Invalid Id",
                    ResponseType = ResponseType.ValidationError
                };
            }

            var participant = await _participantReadRepository.GetEntityByIdAsync(userId, request.EventId);
            if (participant == null)
            {
                return new AddMessageCommandResponse
                {
                    Success = false,
                    Message = "Unauthorized",
                    ResponseType = ResponseType.Unauthorized
                };
            }

            var _event = await _eventReadRepository.GetEntityByIdAsync(request.EventId);

            var message = new Domain.Entities.Message
            {
                Id = Guid.NewGuid(),
                EventId = request.EventId,
                SenderId = userId,
                Text = request.Message,
                SendingTime = DateTime.UtcNow,
            };

            await _messageWriteRepository.AddAsync(message);
            await _messageWriteRepository.SaveChangesAsync();

            var eventParticipants = await _participantReadRepository.GetParticipantsByEventIdAsync(request.EventId);

            var notificationMessage = $"Yeni mesaj var! '{_event.Name}': {request.Message}";

            Domain.Entities.Notification notification = new Domain.Entities.Notification()
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Now,
                IsRead = false,
                Message = notificationMessage,
                UserId = userId,
            };
            foreach (var p in eventParticipants)
            {
                if (p.Id != userId)
                {
                    notification.Id = Guid.NewGuid();
                    notification.UserId = p.Id;
                    await _notificationService.SendNotificationAsync(p.Id.ToString(), notificationMessage);
                    await _notificationWriteRepository.AddAsync(notification);
                }
            }
            await _notificationWriteRepository.SaveChangesAsync();
            return new AddMessageCommandResponse
            {
                Success = true,
                Message = "Message Added",
                ResponseType = ResponseType.Success
            };
        }

    }
}
