using EventGoAPI.Application.Features.Command.Message.AddMessage;
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
            return response.Success ? Ok(response) : Unauthorized(response);
        }

    }
}
