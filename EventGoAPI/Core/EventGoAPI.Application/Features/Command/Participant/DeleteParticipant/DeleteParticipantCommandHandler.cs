using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Application.Enums;
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
                return new DeleteParticipantCommandResponse
                {
                    Success = false,
                    Message = "User ID could not be found or is not a valid GUID.",
                    ResponseType = ResponseType.Unauthorized
                };
            }

            var test1 = await _eventReadRepository.GetEntityByIdAsync(request.EventId);

            if (test1 == null)
            {
                return new DeleteParticipantCommandResponse
                {
                    Success = false,
                    Message = "Wrong Event ID",
                    ResponseType = ResponseType.NotFound
                };
            }

            if (test1.Date < DateTime.Now)
            {
                return new DeleteParticipantCommandResponse
                {
                    Success = false,
                    Message = "Event has already ended.",
                    ResponseType = ResponseType.ValidationError
                };
            }

            if (test1.CreatedById == userId)
            {
                return new DeleteParticipantCommandResponse
                {
                    Success = false,
                    Message = "You are the creator of this event and cannot delete yourself as a participant.",
                    ResponseType = ResponseType.ValidationError
                };
            }

            var test2 = await _participantReadRepository.GetEntityByIdAsync(userId, request.EventId);
            if (test2 == null)
            {
                return new DeleteParticipantCommandResponse
                {
                    Success = false,
                    Message = "Participant does not exist in this event.",
                    ResponseType = ResponseType.Conflict
                };
            }

            await _participantWriteRepository.DeleteAsync(userId, request.EventId);
            await _participantWriteRepository.SaveChangesAsync();

            return new DeleteParticipantCommandResponse
            {
                Success = true,
                Message = "Successfully removed from event.",
                ResponseType = ResponseType.Success
            };
        }
    }
}
