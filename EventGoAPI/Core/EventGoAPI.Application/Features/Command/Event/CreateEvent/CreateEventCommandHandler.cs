﻿using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Application.Enums;
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
        private IPointWriteRepository _pointWriteRepository;
        public CreateEventCommandHandler(IEventWriteRepository eventWriteRepository, IParticipantWriteRepository participantWriteRepository, IHttpContextAccessor httpContextAccessor, IPointWriteRepository pointWriteRepository)
        {
            _eventWriteRepository = eventWriteRepository;
            _participantWriteRepository = participantWriteRepository;
            _httpContextAccessor = httpContextAccessor;
            _pointWriteRepository = pointWriteRepository;
        }

        public async Task<CreateEventCommandResponse> Handle(CreateEventCommandRequest request, CancellationToken cancellationToken)
        {
            if (_httpContextAccessor.HttpContext?.Items["UserId"] is not Guid userId)
            {
                return new CreateEventCommandResponse
                {
                    Success = false,
                    Message = "Invalid Id",
                    ResponseType = ResponseType.ValidationError
                };
            }

            if (request.Date < DateTime.Now)
            {
                return new CreateEventCommandResponse
                {
                    Success = false,
                    Message = "Invalid Date",
                    ResponseType = ResponseType.ValidationError
                };
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
                isApproved = false,
                Image = request.Image,
            };

            var newParticipant = new Domain.Entities.Participant
            {
                EventId = eventId,
                Id = userId
            };

            await _eventWriteRepository.AddAsync(newEvent);
            await _participantWriteRepository.AddAsync(newParticipant);

            await _eventWriteRepository.SaveChangesAsync();

            var newPoint = new Point
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                EventId = eventId,
                Score = 15,
                Date = DateTime.Now,
            };
            await _pointWriteRepository.AddAsync(newPoint);
            await _pointWriteRepository.SaveChangesAsync();

            return new CreateEventCommandResponse
            {
                Success = true,
                Message = "Event Added Successfully",
                ResponseType = ResponseType.Success,
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
                isApproved = newEvent.isApproved,
                Point = newPoint.Score
            };
        }
    }
}
