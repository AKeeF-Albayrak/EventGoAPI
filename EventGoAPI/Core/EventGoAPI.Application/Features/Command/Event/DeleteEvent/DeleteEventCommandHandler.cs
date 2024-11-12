using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Application.Enums;
using EventGoAPI.Domain.Enums;
using MediatR;
using MediatR.Wrappers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.Event.DeleteEvent
{
    public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommandRequest, DeleteEventCommandResponse>
    {
        private IEventWriteRepository _eventWriteRepository;
        private IEventReadRepository _eventReadRepository;
        private IParticipantWriteRepository _participantWriteRepository;
        private IParticipantReadRepository _participantReadRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DeleteEventCommandHandler(IEventWriteRepository eventWriteRepository, IEventReadRepository eventReadRepository, IHttpContextAccessor httpContextAccessor, IParticipantWriteRepository participantWriteRepository, IParticipantReadRepository participantReadRepository)
        {
            _eventWriteRepository = eventWriteRepository;
            _eventReadRepository = eventReadRepository;
            _participantWriteRepository = participantWriteRepository;
            _participantReadRepository = participantReadRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<DeleteEventCommandResponse> Handle(DeleteEventCommandRequest request, CancellationToken cancellationToken)
        {
            var _event = await _eventReadRepository.GetEntityByIdAsync(request.Id);

            if (_httpContextAccessor.HttpContext?.Items["UserId"] is not Guid userId)
            {
                return new DeleteEventCommandResponse
                {
                    Success = false,
                    Message = "User ID could not be found or is not a valid GUID.",
                    ResponseType = ResponseType.Unauthorized,
                    DeletedParticipant = 0
                };
            }

            if (_event == null)
            {
                return new DeleteEventCommandResponse
                {
                    Success = false,
                    Message = "Event not found.",
                    ResponseType = ResponseType.NotFound,
                    DeletedParticipant = 0
                };
            }

            var userRole = _httpContextAccessor.HttpContext?.Items["UserRole"] as string;

            if (userId != _event.CreatedById && userRole != "admin")
            {
                return new DeleteEventCommandResponse
                {
                    Success = false,
                    Message = $"User {userId} does not have permission to delete this event.",
                    ResponseType = ResponseType.Unauthorized,
                    DeletedParticipant = 0
                };
            }

            var participants = await _participantReadRepository.GetParticipantsByEventIdAsync(_event.Id);
            foreach (var participant in participants)
            {
                await _participantWriteRepository.DeleteAsync(participant.Id);
            }
            await _participantWriteRepository.SaveChangesAsync();

            await _eventWriteRepository.DeleteAsync(request.Id);
            await _eventWriteRepository.SaveChangesAsync();

            return new DeleteEventCommandResponse
            {
                Success = true,
                Message = "Event and participants deleted successfully.",
                ResponseType = ResponseType.Success,
                DeletedParticipant = participants.Count()
            };
        }
    }
}
