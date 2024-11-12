using EventGoAPI.API.Utilities;
using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Application.Abstractions.Services;
using EventGoAPI.Application.Enums;
using EventGoAPI.Application.Features.Command.User.CreateUser;
using EventGoAPI.Application.Features.Command.User.Logout;
using EventGoAPI.Application.Features.Command.User.SendEmail;
using EventGoAPI.Application.Features.Command.User.UpdatePassword;
using EventGoAPI.Application.Features.Command.User.VerifyCode;
using EventGoAPI.Application.Features.Query.User.LoginUser;
using EventGoAPI.Domain.Entities;
using EventGoAPI.Persistence.Concretes.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using System.Security.Claims;

namespace EventGoAPI.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginUserQueryRequest loginUserQueryRequest)
        {
            LoginUserQueryResponse response = await _mediator.Send(loginUserQueryRequest);
            return ResponseHandler.CreateResponse(response);
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] CreateUserCommandRequest createUserCommandRequest)
        {
            CreateUserCommandResponse response = await _mediator.Send(createUserCommandRequest);
            return ResponseHandler.CreateResponse(response);
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail([FromBody] SendEmailCommandRequest sendEmailCommandRequest)
        {
            SendEmailCommandResponse response = await _mediator.Send(sendEmailCommandRequest);
            return ResponseHandler.CreateResponse(response);
        }

        [HttpPost]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyCodeCommandRequest verifyCodeCommandRequest)
        {
            VerifyCodeCommandResponse response = await _mediator.Send(verifyCodeCommandRequest);
            return ResponseHandler.CreateResponse(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordCommandRequest updatePasswordCommandRequest)
        {
            UpdatePasswordCommandResponse response = await _mediator.Send(updatePasswordCommandRequest);
            return ResponseHandler.CreateResponse(response);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var response = await _mediator.Send(new LogoutCommandRequest { });
            return ResponseHandler.CreateResponse(response);
        }
    }
}
