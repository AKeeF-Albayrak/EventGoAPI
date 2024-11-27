using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Application.Enums;
using EventGoAPI.Application.Features.Command.Participant.CreateParticipant;
using EventGoAPI.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.FakeData.CreateFakeParticipant
{
    public class CreateFakeParticipantCommandHandler : IRequestHandler<CreateFakeParticipantCommandRequest, CreateFakeParticipantCommandResponse>
    {
        private IParticipantWriteRepository _participantWriteRepository;
        private IParticipantReadRepository _participantReadRepository;
        private IEventReadRepository _eventReadRepository;
        private IPointWriteRepository _pointWriteRepository;
        public CreateFakeParticipantCommandHandler(IParticipantReadRepository participantReadRepository, IParticipantWriteRepository participantWriteRepository, IPointWriteRepository pointWriteRepository, IEventReadRepository eventReadRepository)
        {
            _participantReadRepository = participantReadRepository;
            _participantWriteRepository = participantWriteRepository;
            _pointWriteRepository = pointWriteRepository;
            _eventReadRepository = eventReadRepository;
        }
        public async Task<CreateFakeParticipantCommandResponse> Handle(CreateFakeParticipantCommandRequest request, CancellationToken cancellationToken)
        {
            var eventToJoin = await _eventReadRepository.GetEntityByIdAsync(request.EventId);

            if (eventToJoin.CreatedById == request.UserId)
            {
                return new CreateFakeParticipantCommandResponse()
                {
                    Success = false
                };
            }

            var eventToJoinEndTime = eventToJoin.Date.AddMinutes(eventToJoin.Duration);

            var userCurrentEvents = await _eventReadRepository.GetUsersCurrentEvents(request.UserId);

            foreach (var currentEvent in userCurrentEvents)
            {
                var currentEventEndTime = currentEvent.Date.AddMinutes(currentEvent.Duration);

                if ((eventToJoin.Date < currentEventEndTime) && (eventToJoinEndTime > currentEvent.Date))
                {
                    return new CreateFakeParticipantCommandResponse()
                    {
                        Success = false
                    };
                }
            }

            var existingParticipant = await _participantReadRepository.GetEntityByIdAsync(request.UserId, request.EventId);
            if (existingParticipant != null)
            {
                return new CreateFakeParticipantCommandResponse()
                {
                    Success = false
                };
            }

            var participant = new Domain.Entities.Participant
            {
                Id = request.UserId,
                EventId = request.EventId,
            };

            var point = new Point
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                EventId = request.EventId,
                Score = 10,
                Date = DateTime.UtcNow,
            };

            if (await _participantReadRepository.HasNoParticipationAsync(request.UserId))
            {
                var firstParticipationPoint = new Point
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    EventId = request.EventId,
                    Score = 20,
                    Date = DateTime.UtcNow,
                };
                await _pointWriteRepository.AddAsync(firstParticipationPoint);
            }

            await _participantWriteRepository.AddAsync(participant);
            await _participantWriteRepository.SaveChangesAsync();

            await _pointWriteRepository.AddAsync(point);
            await _pointWriteRepository.SaveChangesAsync();

            return new CreateFakeParticipantCommandResponse
            {
                Success = true,
                Participant = participant,
            };
        }
    }
}
