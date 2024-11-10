using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.User.VerifyCode
{
    public class VerifyCodeCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
