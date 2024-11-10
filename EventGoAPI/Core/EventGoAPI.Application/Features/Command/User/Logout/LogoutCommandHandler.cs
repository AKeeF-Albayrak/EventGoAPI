using EventGoAPI.Application.Abstractions.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.User.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommandRequest, LogoutCommandResponse>
    {
        private readonly ITokenService _tokenService;
        private IHttpContextAccessor _httpContextAccessor;

        public LogoutCommandHandler(ITokenService tokenService, IHttpContextAccessor httpContextAccessor)
        {
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<LogoutCommandResponse> Handle(LogoutCommandRequest request, CancellationToken cancellationToken)
        {
            var token = _httpContextAccessor.HttpContext?.Items["Token"] as string;

            if (string.IsNullOrEmpty(token))
            {
                return new LogoutCommandResponse
                {
                    Success = false,
                    Message = "Token is missing."
                };
            }

            await _tokenService.AddToBlacklistAsync(token);

            return new LogoutCommandResponse
            {
                Success = true,
                Message = "Successfully logged out."
            };
        }
    }
}
