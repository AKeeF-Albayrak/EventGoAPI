using EventGoAPI.API.Utilities;
using EventGoAPI.Application.Features.Command.Feedback.AddFeedback;
using EventGoAPI.Application.Features.Command.Feedback.ReadFeedback;
using EventGoAPI.Application.Features.Query.Feedback.GetFeedbacks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventGoAPI.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FeedbackController : Controller
    {
        private IMediator _mediator;
        public FeedbackController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> GetFeedBacks([FromBody] GetFeedbacksQueryRequest getFeedbacksQueryRequest)
        {
            GetFeedbacksQueryResponse response = await _mediator.Send(getFeedbacksQueryRequest);
            return ResponseHandler.CreateResponse(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SendFeedBack([FromBody]AddFeedbackCommandRequest addFeedbackRequest)
        {
            AddFeedbackCommandResponse response = await _mediator.Send(addFeedbackRequest);
            return ResponseHandler.CreateResponse(response);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ReadFeedBack([FromBody]ReadFeedbackCommandRequest readFeedbackRequest)
        {
            ReadFeedbackCommandResponse response = await _mediator.Send(readFeedbackRequest);
            return ResponseHandler.CreateResponse(response);
        }
    }
}
