using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Application.Abstractions.Services;
using EventGoAPI.Application.Enums;
using EventGoAPI.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.Event.ApproveEvent
{
    public class ApproveEventCommandHandler : IRequestHandler<ApproveEventCommandRequest, ApproveEventCommandResponse>
    {
        private IEventWriteRepository _eventWriteRepository;
        private IEventReadRepository _eventReadRepository;
        private readonly INotificationService _notificationService;
        private INotificationWriteRepository _notificationWriteRepository;
        public ApproveEventCommandHandler(IEventWriteRepository eventWriteRepository, IEventReadRepository eventReadRepository, INotificationService notificationService, INotificationWriteRepository notificationWriteRepository)
        {
            _eventWriteRepository = eventWriteRepository;
            _eventReadRepository = eventReadRepository;
            _notificationService = notificationService;
            _notificationWriteRepository = notificationWriteRepository;
        }
        public async Task<ApproveEventCommandResponse> Handle(ApproveEventCommandRequest request, CancellationToken cancellationToken)
        {
            var _event = await _eventReadRepository.GetEntityByIdAsync(request.Id);

            if (_event == null)
            {
                return new ApproveEventCommandResponse
                {
                    Success = false,
                    Message = "Event not found.",
                    ResponseType = ResponseType.NotFound
                };
            }

            if (_event.isApproved)
            {
                return new ApproveEventCommandResponse
                {
                    Success = true,
                    Message = "Event approval status is already set to the requested value.",
                    ResponseType = ResponseType.Success
                };
            }

            if (!string.IsNullOrEmpty(_event.CreatedById.ToString()))
            {
                var notificationMessage = $"Your event '{_event.Name}' has been approved!";
                await _notificationService.SendNotificationAsync(_event.CreatedById.ToString(), notificationMessage);
                Notification notification = new Notification()
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.Now,
                    IsRead = false,
                    Message = notificationMessage,
                    UserId = _event.CreatedById
                };
                await _notificationWriteRepository.AddAsync(notification);
                await _notificationWriteRepository.SaveChangesAsync();
            }

            _event.isApproved = true;
            await _eventWriteRepository.UpdateAsync(_event);
            await _eventWriteRepository.SaveChangesAsync();

            return new ApproveEventCommandResponse
            {
                Success = true,
                Message = "Event approval status updated successfully.",
                ResponseType = ResponseType.Success
            };
        }
    }
}
