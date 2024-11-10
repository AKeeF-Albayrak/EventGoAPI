using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.User.Logout
{
    public class LogoutCommandRequest : IRequest<LogoutCommandResponse>
    {
    }
}
