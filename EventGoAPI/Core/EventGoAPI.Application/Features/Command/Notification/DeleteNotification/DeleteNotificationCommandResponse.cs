﻿using EventGoAPI.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.Notification.DeleteNotification
{
    public class DeleteNotificationCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ResponseType ResponseType { get; set; }
    }
}