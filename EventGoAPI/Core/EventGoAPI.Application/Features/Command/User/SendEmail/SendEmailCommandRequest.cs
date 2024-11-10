using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.User.SendEmail
{
    public class SendEmailCommandRequest : IRequest<SendEmailCommandResponse>
    {
        public string Email { get; set; }
    }
}
