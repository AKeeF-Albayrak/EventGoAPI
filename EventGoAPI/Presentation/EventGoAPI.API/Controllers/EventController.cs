using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Application.Dtos.EventDtos;
using EventGoAPI.Domain.Entities;
using EventGoAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventGoAPI.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventReadRepository _eventReadRepository;
        private readonly IEventWriteRepository _eventWriteRepository;
        public EventController(IEventWriteRepository eventWriteRepository, IEventReadRepository eventReadRepository)
        {
            _eventWriteRepository = eventWriteRepository;
            _eventReadRepository = eventReadRepository;
        }


        [HttpPost]
        public async Task<IActionResult> AddEvent([FromBody] EventAddDto dto)
        {
            Event _event = new Event
            {
                Id = Guid.NewGuid(),
                //userid
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
                Image = dto.Image,
                CreatedTime = DateTime.Now,
            };

            await _eventWriteRepository.AddAsync(_event);
            await _eventWriteRepository.SaveChangesAsync();
            return Ok(_event);
        }
    }
}
