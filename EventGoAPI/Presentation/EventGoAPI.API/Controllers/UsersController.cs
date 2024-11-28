using EventGoAPI.API.Utilities;
using EventGoAPI.Application.Features.Command.User.DeleteUser;
using EventGoAPI.Application.Features.Command.User.UpdateUser;
using EventGoAPI.Application.Features.Query.Notification;
using EventGoAPI.Application.Features.Query.User.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventGoAPI.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetNotifications([FromQuery] GetNotificationsQueryRequest getNotificationsQueryRequest)
        {
            GetNotificationsQueryResponse response = await _mediator.Send(getNotificationsQueryRequest);
            return ResponseHandler.CreateResponse(response);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUsersAsync([FromQuery] GetUsersQueryRequest getUsersQueryRequest)
        {
            GetUsersQueryResponse response = await _mediator.Send(getUsersQueryRequest);
            return ResponseHandler.CreateResponse(response);
        }

        [HttpDelete]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUserAsync([FromBody] DeleteUserCommandRequest deleteUserCommandRequest)
        {
            DeleteUserCommandResponse response = await _mediator.Send(deleteUserCommandRequest);
            return ResponseHandler.CreateResponse(response);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserCommandRequest updateUserCommandRequest)
        {
            UpdateUserCommandResponse response = await _mediator.Send(updateUserCommandRequest);
            return ResponseHandler.CreateResponse(response);
        }

    }
}
