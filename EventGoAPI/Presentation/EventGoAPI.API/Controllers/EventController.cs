using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Application.Dtos.EventDtos;
using EventGoAPI.Domain.Entities;
using EventGoAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace EventGoAPI.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventReadRepository _eventReadRepository;
        private readonly IEventWriteRepository _eventWriteRepository;
        private readonly IParticipantWriteRepository _participantWriteRepository;
        public EventController(IEventWriteRepository eventWriteRepository, IEventReadRepository eventReadRepository, IParticipantWriteRepository participantWriteRepository)
        {
            _eventWriteRepository = eventWriteRepository;
            _eventReadRepository = eventReadRepository;
            _participantWriteRepository = participantWriteRepository;
        }


        [HttpPost]
        public async Task<IActionResult> AddEvent([FromBody] EventAddDto dto)
        {
            Guid id = Guid.NewGuid();
            Guid createdById = Guid.Parse("72AF2F9FC4D8420A8CBB800A47133318");
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
                //Image = dto.Image,
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

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteEvent(string id)
        {
            await _eventWriteRepository.DeleteAsync(id);
            await _eventWriteRepository.SaveChangesAsync();
            return Ok("Succsessfully deleted!");
        }

        [HttpPost("id")]
        public async Task<IActionResult> ApproveEvent(string id)
        {
            var _event = await _eventReadRepository.GetEntityByIdAsync(id);
            if (_event == null)
            {
                return BadRequest("Cannot Find An Id!");
            }
            
            _event.isApproved = true;
            await _eventWriteRepository.UpdateAsync(_event);
            await _eventWriteRepository.SaveChangesAsync();
            return Ok(_event);
        }

    }
}
