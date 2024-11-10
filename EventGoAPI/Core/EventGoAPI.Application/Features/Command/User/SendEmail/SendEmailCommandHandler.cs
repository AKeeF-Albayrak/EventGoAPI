using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.User.SendEmail
{
    public class SendEmailCommandHandler : IRequestHandler<SendEmailCommandRequest, SendEmailCommandResponse>
    {
        private IEmailService _emailService;
        private IUserReadRepository _userReadRepository;
        private IUserWriteRepository _userWriteRepository;
        public SendEmailCommandHandler(IEmailService emailService, IUserReadRepository userReadRepository, IUserWriteRepository userWriteRepository)
        {
            _emailService = emailService;
            _userReadRepository = userReadRepository;
            _userWriteRepository = userWriteRepository;
        }
        public async Task<SendEmailCommandResponse> Handle(SendEmailCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userReadRepository.CheckUserByUsernameEmailPhoneNumberAsync("", request.Email, "");
            if (user == null)
            {
                return new SendEmailCommandResponse { Success = false, Message = "Email Not Found" };
            }

            var random = new Random();
            string verificationCode = random.Next(100000, 999999).ToString();

            await _emailService.SendEmailAsync(request.Email, verificationCode);
            user.VerificationCode = verificationCode;
            user.VerificationCodeExpiration = DateTime.Now.AddMinutes(3);

            await _userWriteRepository.UpdateAsync(user);
            await _userWriteRepository.SaveChangesAsync();

            return new SendEmailCommandResponse { Success = true, Message = "Verification Code Sent!" };
        }
    }
}
