using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Application.Abstractions.Services;
using EventGoAPI.Application.Enums;
using MediatR;
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
        public LoginUserQueryHandler(IUserReadRepository userReadRepository, ITokenService tokenService, IPasswordHasher passwordHasher)
        {
            _userReadRepository = userReadRepository;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }
        public async Task<LoginUserQueryResponse> Handle(LoginUserQueryRequest request, CancellationToken cancellationToken)
        {
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
