using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Application.Enums;
using EventGoAPI.Application.Features.Command.Participant.DeleteParticipant;
using EventGoAPI.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.Participant.CreateParticipant
{
    public class CreateParticipantCommandHandler : IRequestHandler<CreateParticipantCommandRequest, CreateParticipantCommandResponse>
    {
        private IParticipantWriteRepository _participantWriteRepository;
        private IParticipantReadRepository _participantReadRepository;
        private IEventReadRepository _eventReadRepository;
        private IHttpContextAccessor _httpContextAccessor;
        private IPointWriteRepository _pointWriteRepository;

        public CreateParticipantCommandHandler(IParticipantReadRepository participantReadRepository, IParticipantWriteRepository participantWriteRepository, IHttpContextAccessor httpContextAccessor, IPointWriteRepository pointWriteRepository, IEventReadRepository eventReadRepository)
        {
            _participantReadRepository = participantReadRepository;
            _participantWriteRepository = participantWriteRepository;
            _httpContextAccessor = httpContextAccessor;
            _pointWriteRepository = pointWriteRepository;
            _eventReadRepository = eventReadRepository;
        }
        public async Task<CreateParticipantCommandResponse> Handle(CreateParticipantCommandRequest request, CancellationToken cancellationToken)
        {
            if (_httpContextAccessor.HttpContext?.Items["UserId"] is not Guid userId)
            {
                return new CreateParticipantCommandResponse
                {
                    Success = false,
                    Message = "User ID could not be found or is not a valid GUID.",
                    ResponseType = ResponseType.Unauthorized
                };
            }

            var eventToJoin = await _eventReadRepository.GetEntityByIdAsync(request.EventId);
            if (eventToJoin == null)
            {
                return new CreateParticipantCommandResponse
                {
                    Success = false,
                    Message = "Wrong Event Id!",
                    ResponseType = ResponseType.NotFound
                };
            }

            if (!eventToJoin.isApproved)
            {
                return new CreateParticipantCommandResponse
                {
                    Success = false,
                    Message = "This Event Not Approved",
                    ResponseType = ResponseType.ValidationError
                };
            }

            if (eventToJoin.Date < DateTime.Now)
            {
                return new CreateParticipantCommandResponse
                {
                    Success = false,
                    Message = "This Event Ended!",
                    ResponseType = ResponseType.ValidationError
                };
            }

            if (eventToJoin.CreatedById == userId)
            {
                return new CreateParticipantCommandResponse
                {
                    Success = false,
                    Message = "You Are The Creator of This Event!",
                    ResponseType = ResponseType.ValidationError
                };
            }

            var eventToJoinEndTime = eventToJoin.Date.AddMinutes(eventToJoin.Duration);

            var userCurrentEvents = await _eventReadRepository.GetUsersCurrentEventsAsync(userId);

            foreach (var currentEvent in userCurrentEvents)
            {
                var currentEventEndTime = currentEvent.Date.AddMinutes(currentEvent.Duration);

                if ((eventToJoin.Date < currentEventEndTime) && (eventToJoinEndTime > currentEvent.Date))
                {
                    return new CreateParticipantCommandResponse
                    {
                        Success = false,
                        Message = "You are already registered for another event that conflicts with this event's timing.",
                        ResponseType = ResponseType.Conflict
                    };
                }
            }

            var existingParticipant = await _participantReadRepository.GetEntityByIdAsync(userId, request.EventId);
            if (existingParticipant != null)
            {
                return new CreateParticipantCommandResponse
                {
                    Success = false,
                    Message = "Participant Already Exists",
                    ResponseType = ResponseType.Conflict
                };
            }

            var participant = new Domain.Entities.Participant
            {
                Id = userId,
                EventId = request.EventId,
            };

            var point = new Point
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                EventId = request.EventId,
                Score = 10,
                Date = DateTime.Now,
            };

            if (await _participantReadRepository.HasNoParticipationAsync(userId))
            {
                var firstParticipationPoint = new Point
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    EventId = request.EventId,
                    Score = 20,
                    Date = DateTime.Now,
                };
                await _pointWriteRepository.AddAsync(firstParticipationPoint);
            }

            await _participantWriteRepository.AddAsync(participant);
            await _participantWriteRepository.SaveChangesAsync();

            await _pointWriteRepository.AddAsync(point);
            await _pointWriteRepository.SaveChangesAsync();

            return new CreateParticipantCommandResponse
            {
                Success = true,
                Message = "Participant Added Successfully",
                ResponseType = ResponseType.Success
            };
        }

    }
}
