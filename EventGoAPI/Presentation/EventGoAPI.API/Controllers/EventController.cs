using EventGoAPI.API.Hubs;
using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Application.Abstractions.Services;
using EventGoAPI.Application.Dtos.EventDtos;
using EventGoAPI.Application.Features.Command.Event.CreateEvent;
using EventGoAPI.Application.Features.Command.Event.DeleteEvent;
using EventGoAPI.Application.Features.Command.Participant.CreateParticipant;
using EventGoAPI.Application.Features.Command.Participant.DeleteParticipant;
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
            CreateEventCommandResponse response = await _mediator.Send(createEventCommandRequest);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEvent([FromQuery] DeleteEventCommandRequest deleteEventCommandRequest)
        {
            DeleteEventCommandResponse response = await _mediator.Send(deleteEventCommandRequest);
            return Ok(response);
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> JoinEvent([FromQuery] CreateParticipantCommandRequest createParticipantCommandRequest)
        {
            CreateParticipantCommandResponse response = await _mediator.Send(createParticipantCommandRequest);
            return Ok(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> LeaveEvent([FromQuery] DeleteParticipantCommandRequest deleteParticipantCommandRequest)
        {
            DeleteParticipantCommandResponse response = await _mediator.Send(deleteParticipantCommandRequest);
            return Ok(response);
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
