using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Application.Abstractions.Services;
using EventGoAPI.Application.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.User.UpdatePassword
{
    public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommandRequest, UpdatePasswordCommandResponse>
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UpdatePasswordCommandHandler(IUserReadRepository userReadRepository, IUserWriteRepository userWriteRepository, IPasswordHasher passwordHasher)
        {
            _userReadRepository = userReadRepository;
            _userWriteRepository = userWriteRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<UpdatePasswordCommandResponse> Handle(UpdatePasswordCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userReadRepository.CheckUserByUsernameEmailPhoneNumberAsync("", request.Email, "");

            if (user == null)
            {
                return new UpdatePasswordCommandResponse
                {
                    Success = false,
                    Message = "User not found.",
                    ResponseType = ResponseType.NotFound
                };
            }

            if (!user.PasswordResetAuthorized || user.PasswordResetAuthorizedExpiration < DateTime.Now)
            {
                return new UpdatePasswordCommandResponse
                {
                    Success = false,
                    Message = "You are not authorized to update the password. Please verify the code again.",
                    ResponseType = ResponseType.Unauthorized
                };
            }

            user.PasswordHash = _passwordHasher.HashPassword(request.NewPassword);
            user.PasswordResetAuthorized = false;
            user.PasswordResetAuthorizedExpiration = null;

            await _userWriteRepository.UpdateAsync(user);
            await _userWriteRepository.SaveChangesAsync();

            return new UpdatePasswordCommandResponse
            {
                Success = true,
                Message = "Password updated successfully.",
                ResponseType = ResponseType.Success
            };
        }
    }
}
