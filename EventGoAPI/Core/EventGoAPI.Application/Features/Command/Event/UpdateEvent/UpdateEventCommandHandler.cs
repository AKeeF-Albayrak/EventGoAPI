using EventGoAPI.Application.Abstractions.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.Event.UpdateEvent
{
    public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommandRequest, UpdateEventCommandResponse>
    {
        private IEventWriteRepository _eventWriteRepository;
        private IEventReadRepository _eventReadRepository;
        public UpdateEventCommandHandler(IEventReadRepository eventReadRepository, IEventWriteRepository eventWriteRepository)
        {
            _eventReadRepository = eventReadRepository;
            _eventWriteRepository = eventWriteRepository;
        }
        public async Task<UpdateEventCommandResponse> Handle(UpdateEventCommandRequest request, CancellationToken cancellationToken)
        {
            var existingEvent = await _eventReadRepository.GetEntityByIdAsync(request.Id.ToString());

            if (existingEvent == null)
            {
                return new UpdateEventCommandResponse
                {
                    Success = false,
                    Message = "Event not found."
                };
            }

            if (request.Name != null) existingEvent.Name = request.Name;
            if (request.Description != null) existingEvent.Description = request.Description;
            if (request.Date != null) existingEvent.Date = request.Date.Value;
            if (request.Duration != null) existingEvent.Duration = request.Duration.Value;
            if (request.Address != null) existingEvent.Address = request.Address;
            if (request.City != null) existingEvent.City = request.City;
            if (request.Country != null) existingEvent.Country = request.Country;
            if (request.Latitude != null) existingEvent.Latitude = request.Latitude.Value;
            if (request.Longitude != null) existingEvent.Longitude = request.Longitude.Value;
            if (request.Category != null) existingEvent.Category = request.Category.Value;
            if (request.CreatedTime != null) existingEvent.CreatedTime = request.CreatedTime.Value;

            await _eventWriteRepository.UpdateAsync(existingEvent);
            await _eventWriteRepository.SaveChangesAsync();

            return new UpdateEventCommandResponse
            {
                Success = true,
                Message = "Event updated successfully."
            };
        }
    }
}
