using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Application.Abstractions.Services;
using EventGoAPI.Application.Enums;
using EventGoAPI.Application.Features.Command.Participant.CreateParticipant;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Query.User.LoginUser
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQueryRequest, LoginUserQueryResponse>
    {
        private IUserReadRepository _userReadRepository;
        private IPasswordHasher _passwordHasher;
        private ITokenService _tokenService;
        private IHttpContextAccessor _httpContextAccessor;
        public LoginUserQueryHandler(IUserReadRepository userReadRepository, ITokenService tokenService, IPasswordHasher passwordHasher, IHttpContextAccessor httpContextAccessor)
        {
            _userReadRepository = userReadRepository;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<LoginUserQueryResponse> Handle(LoginUserQueryRequest request, CancellationToken cancellationToken)
        {
            if (_httpContextAccessor.HttpContext?.Items["UserId"] is Guid userId)
            {
                return new LoginUserQueryResponse
                {
                    Success = false,
                    Message = "You Are Already Logged.",
                    ResponseType = ResponseType.Unauthorized
                };
            }
            var user = await _userReadRepository.GetUserByUsernameAsync(request.Username);

            if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                return new LoginUserQueryResponse
                {
                    Success = false,
                    Message = "Invalid username or password.",
                    ResponseType = ResponseType.Unauthorized
                };
            }

            var token = _tokenService.GenerateToken(user);

            return new LoginUserQueryResponse
            {
                Success = true,
                User = user,
                Token = token,
                ResponseType = ResponseType.Success
            };
        }
    }
}
