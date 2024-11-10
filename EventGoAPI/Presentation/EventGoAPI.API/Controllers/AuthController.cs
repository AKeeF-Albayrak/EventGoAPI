using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Application.Abstractions.Services;
using EventGoAPI.Application.Dtos.AuthDtos;
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
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IMediator _mediator;
        public AuthController(IUserReadRepository userReadRepository, IUserWriteRepository userWriteRepository, IPasswordHasher passwordHasher, ITokenService tokenService, IEmailService emailService, IMediator mediator)
        {
            _userWriteRepository = userWriteRepository;
            _userReadRepository = userReadRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _emailService = emailService;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromQuery] LoginUserQueryRequest loginUserQueryRequest)
        {
            LoginUserQueryResponse response = await _mediator.Send(loginUserQueryRequest);
            return response.Success ? Ok(response) : Conflict(new { message = response.Message });
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromQuery] CreateUserCommandRequest createUserCommandRequest)
        {
            CreateUserCommandResponse response = await _mediator.Send(createUserCommandRequest);
            return response.Success ? Ok(response) : Conflict(new { message = response.Message });
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail([FromQuery] SendEmailCommandRequest sendEmailCommandRequest)
        {
            SendEmailCommandResponse response = await _mediator.Send(sendEmailCommandRequest);
            return response.Success ? Ok(response) : Conflict(new { message = response.Message });
        }

        [HttpPost]
        public async Task<IActionResult> VerifyCode([FromQuery] VerifyCodeCommandRequest verifyCodeCommandRequest)
        {
            VerifyCodeCommandResponse response = await _mediator.Send(verifyCodeCommandRequest);
            return response.Success ? Ok(response) : Conflict(new { message = response.Message });
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword([FromQuery] UpdatePasswordCommandRequest updatePasswordCommandRequest)
        {
            UpdatePasswordCommandResponse response = await _mediator.Send(updatePasswordCommandRequest);
            return response.Success ? Ok(response) : Conflict(new { message = response.Message });
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var response = await _mediator.Send(new LogoutCommandRequest {});
            return response.Success ? Ok(response) : BadRequest(response.Message);
        }
    }
}
