using EventGoAPI.API.Utilities;
using EventGoAPI.Application.Features.Command.Message.AddMessage;
using EventGoAPI.Application.Features.Query.Message.GetEventMessages;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventGoAPI.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MessageController : Controller
    {
        private readonly IMediator _mediator;
        public MessageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SendMessage([FromBody] AddMessageCommandRequest addMessageCommandRequest)
        {
            AddMessageCommandResponse response = await _mediator.Send(addMessageCommandRequest);
            return ResponseHandler.CreateResponse(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetMessages([FromBody] GetEventMessagesRequest getEventMessagesRequest)
        {
            GetEventMessagesResponse response = await _mediator.Send(getEventMessagesRequest);
            return ResponseHandler.CreateResponse(response);
        }
    }
}
