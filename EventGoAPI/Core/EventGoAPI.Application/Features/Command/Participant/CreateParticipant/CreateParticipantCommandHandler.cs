using EventGoAPI.Application.Abstractions.Repositories;
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
                return new CreateParticipantCommandResponse
                {
                   Success = false,
                   Message = "Wrong Event Id!"
                };
            }
            if (!test1.isApproved)
            {
                return new CreateParticipantCommandResponse
                {
                    Success = false,
                    Message = "This Event Not Approved"
                };
            }
            if(test1.Date < DateTime.Now)
            {
                return new CreateParticipantCommandResponse
                {
                    Success = false,
                    Message = "This Event Ended!"
                };
            }

            if (test1.CreatedById == userId)
            {
                return new CreateParticipantCommandResponse
                {
                    Success = false,
                    Message = "You Are The Creator This Event!"
                };
            }

            var test2 = await _participantReadRepository.GetEntityByIdAsync(userId.ToString(), request.EventId.ToString());
            if(test2 != null)
            {
                return new CreateParticipantCommandResponse
                {
                    Success = false,
                    Message = "Participant Already Exists"
                };
            }

            var point = new Point
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                EventId = request.EventId,
                Score = 10,
                Date = DateTime.UtcNow,
            };

            if(await _participantReadRepository.HasNoParticipationAsync(userId))
            {
                var point2 = new Point
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    EventId = request.EventId,
                    Score = 20,
                    Date = DateTime.UtcNow,
                };
                await _pointWriteRepository.AddAsync(point2);
            }

            await _participantWriteRepository.AddAsync(participant);
            await _participantWriteRepository.SaveChangesAsync();

            await _pointWriteRepository.AddAsync(point);
            await _pointWriteRepository.SaveChangesAsync();

            return new CreateParticipantCommandResponse()
            {
                Success = true,
                Message = "Participant Added Successfully"
            };
        }
    }
}
