using EventGoAPI.API.Utilities;
using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Application.Abstractions.Services;
using EventGoAPI.Application.Features.Command.Event.ApproveEvent;
using EventGoAPI.Application.Features.Command.Event.CreateEvent;
using EventGoAPI.Application.Features.Command.Event.DeleteEvent;
using EventGoAPI.Application.Features.Command.Event.UpdateEvent;
using EventGoAPI.Application.Features.Command.Participant.CreateParticipant;
using EventGoAPI.Application.Features.Command.Participant.DeleteParticipant;
using EventGoAPI.Application.Features.Query.Event.GetAllEvents;
using EventGoAPI.Application.Features.Query.Event.GetApprovedEvents;
using EventGoAPI.Application.Features.Query.Event.GetCreatedEvents;
using EventGoAPI.Application.Features.Query.Event.GetCurrentEvents;
using EventGoAPI.Application.Features.Query.Event.GetEventById;
using EventGoAPI.Application.Features.Query.Event.GetPastEvents;
using EventGoAPI.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations.Schema;
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
        public async Task<IActionResult> GetEventById([FromQuery]GetEventByIdQueryRequest getEventByIdQueryRequest)
        {
            GetEventByIdQueryResponse response = await _mediator.Send(getEventByIdQueryRequest);
            return ResponseHandler.CreateResponse(response);
        }

        [HttpGet]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> GetAllEventsForAdmin([FromQuery] GetAllEventsQueryRequest getAllEventsQueryRequest)
        {
            GetAllEventsQueryResponse response = await _mediator.Send(getAllEventsQueryRequest);
            return ResponseHandler.CreateResponse(response);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetEventsForUser([FromQuery] GetApprovedEventQueryRequest getApprovedEventQueryRequest)
        {
            GetApprovedEventQueryResponse response = await _mediator.Send(getApprovedEventQueryRequest);
            return ResponseHandler.CreateResponse(response);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUsersCurrentEvents([FromQuery] GetCurrentEventsQueryRequest getCurrentEventsQueryRequest)
        {
            GetCurrentEventsQueryResponse response = await _mediator.Send(getCurrentEventsQueryRequest);
            return ResponseHandler.CreateResponse(response);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUsersCreatedEvents([FromQuery]GetCreatedEventsQueryRequest getCreatedEventsQueryRequest)
        {
            GetCreatedEventsQueryResponse response = await _mediator.Send(getCreatedEventsQueryRequest);
            return ResponseHandler.CreateResponse(response);
        }

        [HttpGet]   
        [Authorize]
        public async Task<IActionResult> GetUsersPastEvents([FromQuery] GetPastEventsQueryRequest getPastEventsQueryRequest)
        {
            GetPastEventsQueryResponse response = await _mediator.Send(getPastEventsQueryRequest);
            return ResponseHandler.CreateResponse(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddEvent([FromBody] CreateEventCommandRequest createEventCommandRequest)
        {
            CreateEventCommandResponse response = await _mediator.Send(createEventCommandRequest);
            return ResponseHandler.CreateResponse(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEvent([FromBody] DeleteEventCommandRequest deleteEventCommandRequest)
        {
            DeleteEventCommandResponse response = await _mediator.Send(deleteEventCommandRequest);
            return ResponseHandler.CreateResponse(response);
        }
        
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ApproveEvent([FromBody] ApproveEventCommandRequest approveEventCommandRequest)
        {
            ApproveEventCommandResponse response = await _mediator.Send(approveEventCommandRequest);
            return ResponseHandler.CreateResponse(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> JoinEvent([FromBody] CreateParticipantCommandRequest createParticipantCommandRequest)
        {
            CreateParticipantCommandResponse response = await _mediator.Send(createParticipantCommandRequest);
            return ResponseHandler.CreateResponse(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> LeaveEvent([FromBody] DeleteParticipantCommandRequest deleteParticipantCommandRequest)
        {
            DeleteParticipantCommandResponse response = await _mediator.Send(deleteParticipantCommandRequest);
            return ResponseHandler.CreateResponse(response);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateEvent([FromBody] UpdateEventCommandRequest updateEventCommandRequest)
        {
            UpdateEventCommandResponse response = await _mediator.Send(updateEventCommandRequest);
            return ResponseHandler.CreateResponse(response);
        }
    }
}
