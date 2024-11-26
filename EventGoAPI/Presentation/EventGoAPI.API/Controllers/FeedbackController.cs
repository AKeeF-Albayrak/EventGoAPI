using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventGoAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private IMediator _mediator;
        public FeedbackController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /*[HttpGet]
        [Authorize]
        public async Task<IActionResult> GetFeedBacks()
        {

        }*/
    }
}
