using EventGoAPI.API.Hubs;
using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Application.Abstractions.Services;
using EventGoAPI.Application.Dtos.EventDtos;
using EventGoAPI.Application.Features.Command.Event.ApproveEvent;
using EventGoAPI.Application.Features.Command.Event.CreateEvent;
using EventGoAPI.Application.Features.Command.Event.DeleteEvent;
using EventGoAPI.Application.Features.Command.Event.UpdateEvent;
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
        private readonly IMediator _mediator;
        public EventController(IMediator mediator)
        {
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

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ApproveEvent([FromQuery] ApproveEventCommandRequest approveEventCommandRequest)
        {
            ApproveEventCommandResponse response = await _mediator.Send(approveEventCommandRequest);
            return Ok(response);
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

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateEvent([FromQuery] UpdateEventCommandRequest updateEventCommandRequest)
        {
            UpdateEventCommandResponse response = await _mediator.Send(updateEventCommandRequest);
            return Ok(response);
        }
    }
}
