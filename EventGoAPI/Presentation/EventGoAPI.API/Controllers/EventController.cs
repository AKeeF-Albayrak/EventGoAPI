﻿using EventGoAPI.API.Hubs;
using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Application.Abstractions.Services;
using EventGoAPI.Application.Dtos.EventDtos;
using EventGoAPI.Application.Features.Command.Event.CreateEvent;
using EventGoAPI.Application.Features.Query.Event.GetAllEvents;
using EventGoAPI.Domain.Entities;
using EventGoAPI.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace EventGoAPI.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventReadRepository _eventReadRepository;
        private readonly IEventWriteRepository _eventWriteRepository;
        private readonly IParticipantWriteRepository _participantWriteRepository;
        private readonly IParticipantReadRepository _participantReadRepository;
        private readonly IHubContext<NotificationsHub> _notificationsHub;
        private readonly ITokenService _tokenService;
        private readonly IMediator _mediator;
        public EventController(IEventWriteRepository eventWriteRepository, IEventReadRepository eventReadRepository, IParticipantWriteRepository participantWriteRepository, IParticipantReadRepository participantReadRepository, IHubContext<NotificationsHub> notificationsHub, ITokenService tokenService, IMediator mediator)
        {
            _eventWriteRepository = eventWriteRepository;
            _eventReadRepository = eventReadRepository;
            _participantWriteRepository = participantWriteRepository;
            _participantReadRepository = participantReadRepository;
            _notificationsHub = notificationsHub;
            _tokenService = tokenService;
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetEvents([FromQuery] GetAllEventsQueryRequest getAllEventsQueryRequest)
        {
            GetAllEventsQueryResponse response = await _mediator.Send(getAllEventsQueryRequest);
            return Ok(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddEvent([FromQuery] CreateEventCommandRequest createEventCommandRequest)
        {
            // puan eklicek
            CreateEventCommandResponse response = await _mediator.Send(createEventCommandRequest);
            return Ok(response);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(string id)
        {
            //puani silecek
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out Guid userId))
            {
                return BadRequest("Invalid user ID format.");
            }

            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole == "Admin")
            {
                await _eventWriteRepository.DeleteAsync(id);
                await _eventWriteRepository.SaveChangesAsync();
                return Ok("Successfully deleted!");
            }

            var eventToDelete = await _eventReadRepository.GetEntityByIdAsync(id);
            if (eventToDelete == null)
            {
                return NotFound("Event not found.");
            }

            if (eventToDelete.CreatedById != userId)
            {
                return Forbid("You are not authorized to delete this event.");
            }

            await _eventWriteRepository.DeleteAsync(id);
            await _eventWriteRepository.SaveChangesAsync();
            return Ok("Successfully deleted!");
        }

        [HttpPost("id")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ApproveEvent(string id)
        {
            var _event = await _eventReadRepository.GetEntityByIdAsync(id);
            if (_event == null)
            {
                return BadRequest("Cannot Find An Id!");
            }
            if (_event.isApproved) return Ok(_event);
            _event.isApproved = true;
            await _eventWriteRepository.UpdateAsync(_event);
            await _eventWriteRepository.SaveChangesAsync();

            if (!string.IsNullOrEmpty(_event.CreatedById.ToString()))
            {
                var notificationMessage = $"Your event '{_event.Name}' has been approved!";
                await _notificationsHub.Clients.User(_event.CreatedById.ToString())
                    .SendAsync("ReceiveNotification", notificationMessage);
            }

            return Ok(_event);
        }

        [HttpPost("id")]
        [Authorize]
        public async Task<IActionResult> JoinEvent(string id)
        {
            // zaman kontrolu
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out Guid userId) || !Guid.TryParse(id, out Guid eventId))
            {
                return BadRequest("Invalid user ID format.");
            }

            var _participant = await _participantReadRepository.GetEntityByIdAsync(userIdClaim, id);
            if (_participant != null) return Forbid("You Are Already Exist In This Event!");

            Participant participant = new Participant
            {
                Id = userId,
                EventId = eventId,
            };

            await _participantWriteRepository.AddAsync(participant);
            await _participantWriteRepository.SaveChangesAsync();

            return Ok(participant);
        }

        [HttpPost("id")]
        [Authorize]
        public async Task<IActionResult> LeaveEvent(string id)
        {
            //kendi olusturdugu etkinlikten cikamasin
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out Guid userId) || !Guid.TryParse(id, out Guid eventId))
            {
                return BadRequest("Invalid user ID format.");
            }

            var _participant = await _participantReadRepository.GetEntityByIdAsync(userIdClaim, id);
            if (_participant == null) return Ok("You Are Already Not Exist In This Event!");

            Participant participant = new Participant
            {
                Id = userId,
                EventId = eventId,
            };

            await _participantWriteRepository.DeleteAsync(userIdClaim, id);
            await _participantWriteRepository.SaveChangesAsync();
            return Ok("Succsessfully Deleted!");
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateEvent(string id, [FromBody] EventUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!Guid.TryParse(id, out Guid guidId))
            {
                return BadRequest("Invalid ID format.");
            }

            var existingEvent = await _eventReadRepository.GetEntityByIdAsync(id);
            if (existingEvent == null)
            {
                return NotFound("Event not found.");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out Guid userId))
            {
                return BadRequest("Invalid user ID format.");
            }

            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;


            if (existingEvent.CreatedById != userId && userRole != "admin")
            {
                return Unauthorized();
            }

            Event _event = new Event
            {
                Id = guidId,
                CreatedById = userId,
                Name = dto.Name,
                Description = dto.Description,
                Date = dto.Date,
                Duration = dto.Duration,
                Address = dto.Address,
                City = dto.City,
                Country = dto.Country,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Category = dto.Category,
                Image = existingEvent.Image,
                CreatedTime = existingEvent.CreatedTime,
                isApproved = existingEvent.isApproved,
                CreatedBy = existingEvent.CreatedBy,
                Messages = existingEvent.Messages,
                Participants = existingEvent.Participants,
            };
            await _eventWriteRepository.UpdateAsync(_event);
            await _eventWriteRepository.SaveChangesAsync();

            return Ok(_event);
        }
    }
}
