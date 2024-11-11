using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Application.Features.Command.Participant.CreateParticipant;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.Participant.DeleteParticipant
{
    public class DeleteParticipantCommandHandler : IRequestHandler<DeleteParticipantCommandRequest, DeleteParticipantCommandResponse>
    {
        private IParticipantReadRepository _participantReadRepository;
        private IParticipantWriteRepository _participantWriteRepository;
        private IEventReadRepository _eventReadRepository;
        private IHttpContextAccessor _httpContextAccessor;
        public DeleteParticipantCommandHandler(IParticipantWriteRepository participantWriteRepository, IParticipantReadRepository participantReadRepository, IHttpContextAccessor httpContextAccessor, IEventReadRepository eventReadRepository)
        {
            _participantWriteRepository = participantWriteRepository;
            _participantReadRepository = participantReadRepository;
            _httpContextAccessor = httpContextAccessor;
            _eventReadRepository = eventReadRepository;
        }
        public async Task<DeleteParticipantCommandResponse> Handle(DeleteParticipantCommandRequest request, CancellationToken cancellationToken)
        {
            if (_httpContextAccessor.HttpContext?.Items["UserId"] is not Guid userId)
            {
                throw new UnauthorizedAccessException("User ID could not be found or is not a valid GUID.");
            }

            var participant = new Domain.Entities.Participant
            {
                Id = userId,
                EventId = request.EventId,
            };
            var test1 = await _eventReadRepository.GetEntityByIdAsync(request.EventId.ToString());

            if(test1 == null)
            {
                return new DeleteParticipantCommandResponse
                {
                    Succsess = false,
                    Message = "Wrong EventId"
                };
            }

            if (test1.CreatedById == userId)
            {
                return new DeleteParticipantCommandResponse
                {
                    Succsess = false,
                    Message = "You Are The Creator This Event"
                };
            }

            var test2 = await _participantReadRepository.GetEntityByIdAsync(userId.ToString(), request.EventId.ToString());
            if(test2 == null)
            {
                return new DeleteParticipantCommandResponse
                {
                    Succsess = false,
                    Message = "Participant Already Not Exists!"
                };
            }

            await _participantWriteRepository.DeleteAsync(userId.ToString(), request.EventId.ToString());
            await _participantWriteRepository.SaveChangesAsync();

            return new DeleteParticipantCommandResponse
            {
                Succsess = true
            };
        }
    }
}
