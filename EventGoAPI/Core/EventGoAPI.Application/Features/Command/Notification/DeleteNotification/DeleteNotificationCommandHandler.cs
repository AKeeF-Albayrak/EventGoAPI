using EventGoAPI.Application.Abstractions.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.Notification.DeleteNotification
{
    public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommandRequest,  DeleteNotificationCommandResponse>
    {
        private INotificationWriteRepository _notificationWriteRepository;
        public DeleteNotificationCommandHandler( INotificationWriteRepository notificationWriteRepository)
        {
            _notificationWriteRepository = notificationWriteRepository;
        }

        public async Task<DeleteNotificationCommandResponse> Handle(DeleteNotificationCommandRequest request, CancellationToken cancellationToken)
        {
            await _notificationWriteRepository.DeleteAsync(request.Id);
            return new DeleteNotificationCommandResponse()
            {
                Success = true,
                Message = "Success",
                ResponseType = Enums.ResponseType.Success,
            };
        }
    }
}
