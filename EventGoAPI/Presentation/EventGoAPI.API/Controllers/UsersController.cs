﻿using EventGoAPI.API.Utilities;
using EventGoAPI.Application.Features.Command.User.DeleteUser;
using EventGoAPI.Application.Features.Query.User.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventGoAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUsersAsync([FromBody] GetUsersQueryRequest getUsersQueryRequest)
        {
            GetUsersQueryResponse response = await _mediator.Send(getUsersQueryRequest);
            return ResponseHandler.CreateResponse(response);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUserAsync([FromBody] DeleteUserCommandRequest deleteUserCommandRequest)
        {
            DeleteUserCommandResponse response = await _mediator.Send(deleteUserCommandRequest);
            return ResponseHandler.CreateResponse(response);
        }
    }
}
