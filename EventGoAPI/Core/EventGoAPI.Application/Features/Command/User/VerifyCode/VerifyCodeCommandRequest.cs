using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.User.VerifyCode
{
    public class VerifyCodeCommandRequest : IRequest<VerifyCodeCommandResponse>
    {
        public string Email { get; set; }
        public string EnteredCode { get; set; }
    }
}
