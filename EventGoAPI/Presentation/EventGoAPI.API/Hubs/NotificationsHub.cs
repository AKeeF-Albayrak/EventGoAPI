using EventGoAPI.Application.Abstractions.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace EventGoAPI.API.Hubs
{
    public class NotificationsHub : Hub, INotificationsHub
    {
        public async Task SendNotificationAsync(string userId, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveNotification", message);
        }
    }
}