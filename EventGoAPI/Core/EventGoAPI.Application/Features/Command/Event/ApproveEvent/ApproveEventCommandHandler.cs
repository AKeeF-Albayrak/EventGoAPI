using EventGoAPI.Application.Abstractions.Hubs;
using EventGoAPI.Application.Abstractions.Repositories;
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
        private readonly INotificationsHub _notificationsHub;
        public ApproveEventCommandHandler(IEventWriteRepository eventWriteRepository, IEventReadRepository eventReadRepository, INotificationsHub notificationsHub)
        {
            _eventWriteRepository = eventWriteRepository;
            _eventReadRepository = eventReadRepository;
            _notificationsHub = notificationsHub;
        }
        public async Task<ApproveEventCommandResponse> Handle(ApproveEventCommandRequest request, CancellationToken cancellationToken)
        {
            var _event = await _eventReadRepository.GetEntityByIdAsync(request.Id.ToString());

            if (_event == null)
            {
                return new ApproveEventCommandResponse
                {
                    Success = false,
                    Message = "Event not found."
                };
            }

            if (_event.isApproved == true)
            {
                return new ApproveEventCommandResponse
                {
                    Success = true,
                    Message = "Event approval status is already set to the requested value."
                };
            }

            _event.isApproved = true;
            await _eventWriteRepository.UpdateAsync(_event);
            await _eventWriteRepository.SaveChangesAsync();

            if (!string.IsNullOrEmpty(_event.CreatedById.ToString()))
            {
                var notificationMessage = $"Your event '{_event.Name}' has been approved!";
                await _notificationsHub.SendNotificationAsync(_event.CreatedById.ToString(), notificationMessage);
            }

            return new ApproveEventCommandResponse
            {
                Success = true,
                Message = "Event approval status updated successfully."
            };
        }
    }
}
