using EventGoAPI.API.Utilities;
using EventGoAPI.Application.Features.Query.Point;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventGoAPI.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PointController : Controller
    {
        private IMediator _mediator;
        public PointController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPoints([FromQuery] GetPointsQueryRequest getPointsQueryRequest)
        {
            GetPointsQueryResponse response = await _mediator.Send(getPointsQueryRequest);
            return ResponseHandler.CreateResponse(response);
        }
    }
}
