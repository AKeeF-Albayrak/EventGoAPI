using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Application.Enums;
using EventGoAPI.Application.Features.Command.Event.CreateEvent;
using EventGoAPI.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.FakeData.CreateFakeEvent
{
    public class CreateFakeEventCommandHandler : IRequestHandler<CreateFakeEventCommandRequest, CreateFakeEventCommandResponse>
    {
        private IEventWriteRepository _eventWriteRepository;
        private IParticipantWriteRepository _participantWriteRepository;
        private IPointWriteRepository _pointWriteRepository;
        public CreateFakeEventCommandHandler(IEventWriteRepository eventWriteRepository, IParticipantWriteRepository participantWriteRepository, IPointWriteRepository pointWriteRepository)
        {
            _eventWriteRepository = eventWriteRepository;
            _participantWriteRepository = participantWriteRepository;
            _pointWriteRepository = pointWriteRepository;
        }
        public async Task<CreateFakeEventCommandResponse> Handle(CreateFakeEventCommandRequest request, CancellationToken cancellationToken)
        {
            Guid eventId = Guid.NewGuid();
            var newEvent = new Domain.Entities.Event
            {
                Id = eventId,
                Name = request.Name,
                Description = request.Description,
                Date = request.Date,
                Duration = request.Duration,
                Address = request.Address,
                City = request.City,
                Country = request.Country,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Category = request.Category,
                CreatedTime = DateTime.Now,
                CreatedById = request.CreatedById,
                isApproved = request.IsApproved,
                Image = request.Image,
            };

            var newParticipant = new Domain.Entities.Participant
            {
                EventId = eventId,
                Id = request.CreatedById,
            };

            await _eventWriteRepository.AddAsync(newEvent);
            await _participantWriteRepository.AddAsync(newParticipant);

            await _eventWriteRepository.SaveChangesAsync();

            var newPoint = new Point
            {
                Id = Guid.NewGuid(),
                UserId = request.CreatedById,
                EventId = eventId,
                Score = 15,
                Date = DateTime.Now,
            };
            await _pointWriteRepository.AddAsync(newPoint);
            await _pointWriteRepository.SaveChangesAsync();

            return new CreateFakeEventCommandResponse
            {
                Event = newEvent,
            };
        }
    }
}
