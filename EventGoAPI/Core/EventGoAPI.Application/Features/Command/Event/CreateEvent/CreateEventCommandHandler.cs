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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CreateEventCommandHandler(IEventWriteRepository eventWriteRepository, IParticipantWriteRepository participantWriteRepository, IHttpContextAccessor httpContextAccessor)
        {
            _eventWriteRepository = eventWriteRepository;
            _participantWriteRepository = participantWriteRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CreateEventCommandResponse> Handle(CreateEventCommandRequest request, CancellationToken cancellationToken)
        {
            if (_httpContextAccessor.HttpContext?.Items["UserId"] is not Guid userId)
            {
                throw new UnauthorizedAccessException("User ID could not be found or is not a valid GUID.");
            }


            var newEvent = new Domain.Entities.Event
            {
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
                CreatedTime = request.CreatedTime,
                CreatedById = userId,
                isApproved = false
            };

            await _eventWriteRepository.AddAsync(newEvent);
            await _eventWriteRepository.SaveChangesAsync();

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
