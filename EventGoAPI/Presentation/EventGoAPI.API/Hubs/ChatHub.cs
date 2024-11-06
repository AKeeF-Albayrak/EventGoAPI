using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace EventGoAPI.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IParticipantReadRepository _participantReadRepository;
        private readonly IMessageWriteRepository _messageWriteRepository;

        public ChatHub(IParticipantReadRepository participantReadRepository, IMessageWriteRepository messageWriteRepository)
        {
            _participantReadRepository = participantReadRepository;
            _messageWriteRepository = messageWriteRepository;
        }

        public async Task JoinEventChat(string eventId)
        {
            var userId = Context.UserIdentifier;

            if (await _participantReadRepository.GetEntityByIdAsync(userId, eventId) != null)
            {
                var groupName = $"Event_{eventId}";
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                await Clients.Caller.SendAsync("JoinSuccess", groupName);
            }
            else
            {
                throw new HubException("You are not authorized to join this chat.");
            }
        }

        public async Task LeaveEventChat(string eventId)
        {
            var groupName = $"Event_{eventId}";
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task SendMessageToEventChat(string eventId, string message)
        {
            if (!Guid.TryParse(eventId, out Guid eventGuid))
            {
                throw new ArgumentException("Invalid event ID format.");
            }

            if (!Guid.TryParse(Context.UserIdentifier, out Guid userGuid))
            {
                throw new ArgumentException("Invalid user ID format.");
            }

            var groupName = $"Event_{eventGuid}";

            if (await _participantReadRepository.GetEntityByIdAsync(userGuid.ToString(), eventGuid.ToString()) != null)
            {
                Message _message = new Message
                {
                    SenderId = userGuid,
                    EventId = eventGuid,
                    Text = message,
                    SendingTime = DateTime.UtcNow,
                };

                await _messageWriteRepository.AddAsync(_message);
                await _messageWriteRepository.SaveChangesAsync();

                await Clients.Group(groupName).SendAsync("ReceiveMessage", userGuid.ToString(), message);
            }
            else
            {
                throw new HubException("You are not authorized to send messages in this chat.");
            }
        }

    }
}