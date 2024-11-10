using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.Event.CreateEvent
{
    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommandRequest, CreateEventCommandResponse>
    {
        private IEventWriteRepository _eventWriteRepository;
        private IParticipantWriteRepository _participantWriteRepository;
        private IHttpContextAccessor _httpContextAccessor;
        public CreateEventCommandHandler(IEventWriteRepository eventWriteRepository, IParticipantWriteRepository participantWriteRepository, IHttpContextAccessor httpContextAccessor)
        {
            _eventWriteRepository = eventWriteRepository;
            _participantWriteRepository = participantWriteRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CreateEventCommandResponse> Handle(CreateEventCommandRequest request, CancellationToken cancellationToken)
        {
            //puan ekleme
            if (_httpContextAccessor.HttpContext?.Items["UserId"] is not Guid userId)
            {
                throw new UnauthorizedAccessException("User ID could not be found or is not a valid GUID.");
            }
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
                CreatedById = userId,
                isApproved = false
            };

            var newParticipant = new Domain.Entities.Participant
            {
                EventId = eventId,
                Id = userId
            };

            await _eventWriteRepository.AddAsync(newEvent);
            await _eventWriteRepository.SaveChangesAsync();

            await _participantWriteRepository.AddAsync(newParticipant);
            await _participantWriteRepository.SaveChangesAsync();

            var response = new CreateEventCommandResponse
            {
                Name = newEvent.Name,
                Description = newEvent.Description,
                Date = newEvent.Date,
                Duration = newEvent.Duration,
                Address = newEvent.Address,
                City = newEvent.City,
                Country = newEvent.Country,
                Latitude = newEvent.Latitude,
                Longitude = newEvent.Longitude,
                Category = newEvent.Category,
                CreatedTime = newEvent.CreatedTime,
                isApproved = newEvent.isApproved
            };

            return response;
        }
    }
}
