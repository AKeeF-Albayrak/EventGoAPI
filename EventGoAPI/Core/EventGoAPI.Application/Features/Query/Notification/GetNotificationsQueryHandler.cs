using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Application.Enums;
using EventGoAPI.Application.Features.Command.Event.CreateEvent;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Query.Notification
{
    public class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQueryRequest, GetNotificationsQueryResponse>
    {
        private INotificationReadRepository _notificationReadRepository;
        private IHttpContextAccessor _httpContextAccessor;
        public GetNotificationsQueryHandler(INotificationReadRepository notificationReadRepository, IHttpContextAccessor httpContextAccessor)
        {
            _notificationReadRepository = notificationReadRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<GetNotificationsQueryResponse> Handle(GetNotificationsQueryRequest request, CancellationToken cancellationToken)
        {
            if (_httpContextAccessor.HttpContext?.Items["UserId"] is not Guid userId)
            {
                return new GetNotificationsQueryResponse
                {
                    Success = false,
                    Message = "Invalid Id",
                    ResponseType = ResponseType.ValidationError
                };
            }

            var notifications = await _notificationReadRepository.GetNotificationsAsync(userId);

            return new GetNotificationsQueryResponse
            {
                Success = true,
                Message = "Successfully!",
                ResponseType = ResponseType.Success,
                Notifications = notifications
            };
        }
    }
}
