using EventGoAPI.Application.Enums;
using EventGoAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Query.Notification
{
    public class GetNotificationsQueryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ResponseType ResponseType { get; set; }
        public IEnumerable<Domain.Entities.Notification> Notifications { get; set; }
    }
}
