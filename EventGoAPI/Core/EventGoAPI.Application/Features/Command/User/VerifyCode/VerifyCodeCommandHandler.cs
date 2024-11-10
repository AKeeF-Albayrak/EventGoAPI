using EventGoAPI.Application.Abstractions.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.User.VerifyCode
{
    public class VerifyCodeCommandHandler : IRequestHandler<VerifyCodeCommandRequest, VerifyCodeCommandResponse>
    {
        private IUserReadRepository _userReadRepository;
        private IUserWriteRepository _userWriteRepository;
        public VerifyCodeCommandHandler(IUserReadRepository userReadRepository, IUserWriteRepository userWriteRepository)
        {
            _userReadRepository = userReadRepository;
            _userWriteRepository = userWriteRepository;
        }
        public async Task<VerifyCodeCommandResponse> Handle(VerifyCodeCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userReadRepository.CheckUserByUsernameEmailPhoneNumberAsync("", request.Email, "");

            if (user == null)
            {
                return new VerifyCodeCommandResponse { Success = false, Message = "User not found." };
            }

            if (user.VerificationCodeExpiration < DateTime.Now || user.VerificationCode != request.EnteredCode)
            {
                return new VerifyCodeCommandResponse { Success = false, Message = "Invalid or expired verification code." };
            }

            user.VerificationCode = null;
            user.VerificationCodeExpiration = null;
            user.PasswordResetAuthorized = true;
            user.PasswordResetAuthorizedExpiration = DateTime.Now.AddMinutes(10);

            await _userWriteRepository.UpdateAsync(user);
            await _userWriteRepository.SaveChangesAsync();

            return new VerifyCodeCommandResponse { Success = true, Message = "Verification successful." };
        }
    }
}
