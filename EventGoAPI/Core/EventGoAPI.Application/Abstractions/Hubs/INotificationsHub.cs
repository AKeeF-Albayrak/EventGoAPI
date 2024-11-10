using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Abstractions.Hubs
{
    public interface INotificationsHub
    {
        Task SendNotificationAsync(string userId, string message);
    }
}
