using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.Message.AddMessage
{
    public class AddMessageCommandHandler : IRequestHandler<AddMessageCommandRequest, AddMessageCommandResponse>
    {
        private IMessageWriteRepository _messageWriteRepository;
        private IParticipantReadRepository _participantReadRepository;
        private IHttpContextAccessor _httpContextAccessor;
        public AddMessageCommandHandler(IMessageWriteRepository messageWriteRepository, IHttpContextAccessor httpContextAccessor, IParticipantReadRepository participantReadRepository)
        {
            _messageWriteRepository = messageWriteRepository;
            _httpContextAccessor = httpContextAccessor;
            _participantReadRepository = participantReadRepository;
        }
        public async Task<AddMessageCommandResponse> Handle(AddMessageCommandRequest request, CancellationToken cancellationToken)
        {
            if (_httpContextAccessor.HttpContext?.Items["UserId"] is not Guid userId)
            {
                return new AddMessageCommandResponse
                {
                    Success = false,
                    Message = "Unvalid Id"
                };
            }

            var participant = await _participantReadRepository.GetEntityByIdAsync(userId, request.EventId);

            if (participant == null)
            {
                return new AddMessageCommandResponse
                {
                    Success = false,
                    Message = "Unauthorized"
                };
            }

            var message = new Domain.Entities.Message
            {
                Id = Guid.NewGuid(),
                EventId = request.EventId,
                SenderId = userId,
                Text = request.Message,
                SendingTime = DateTime.UtcNow,
            };

            await _messageWriteRepository.AddAsync(message);
            await _messageWriteRepository.SaveChangesAsync();

            return new AddMessageCommandResponse
            {
                Success = true,
                Message = "Message Added"
            };
        }
    }
}
