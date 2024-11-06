using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Application.Dtos.EventDtos;
using EventGoAPI.Domain.Entities;
using EventGoAPI.Domain.Entities;
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
        public EventController(IEventWriteRepository eventWriteRepository, IEventReadRepository eventReadRepository, IParticipantWriteRepository participantWriteRepository, IParticipantReadRepository participantReadRepository)
        {
            _eventWriteRepository = eventWriteRepository;
            _eventReadRepository = eventReadRepository;
            _participantWriteRepository = participantWriteRepository;
            _participantReadRepository = participantReadRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Event>>> GetEvents()
        {
            var events = await _eventReadRepository.GetAllEventsForUserAsync();

            if (events == null || !events.Any())
            {
                return NotFound("No events found for the user.");
            }

            return Ok(events.ToList());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddEvent([FromBody] EventAddDto dto)
        {
            // puan eklicek
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out Guid createdById))
            {
                return BadRequest("Invalid user ID format.");
            }

            Guid id = Guid.NewGuid();

            Event _event = new Event
            {
                Id = id,
                CreatedById = createdById,
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
                CreatedTime = DateTime.Now,
                isApproved = false,
            };

            Participant _participant = new Participant
            {
                Id = createdById,
                EventId = id,
            };

            await _eventWriteRepository.AddAsync(_event);
            await _eventWriteRepository.SaveChangesAsync();

            await _participantWriteRepository.AddAsync(_participant);
            await _participantWriteRepository.SaveChangesAsync();

            _event.Participants = new List<Participant> { _participant };

            return Ok(_event);
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
            if (_participant != null) return Ok("You Are Already Exist In This Event!");

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
    }
}
