﻿using EventGoAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Abstractions.Repositories
{
    public interface INotificationReadRepository : IReadRepository<Notification>
    {
        public Task<List<Notification>> GetNotificationsAsync(Guid userid);
    }
}