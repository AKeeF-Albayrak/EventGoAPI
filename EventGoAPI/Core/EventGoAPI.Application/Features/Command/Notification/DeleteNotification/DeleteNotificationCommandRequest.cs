using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.Notification.DeleteNotification
{
    public class DeleteNotificationCommandRequest : IRequest<DeleteNotificationCommandResponse>
    {
        public Guid Id { get; set; }
    }
}
