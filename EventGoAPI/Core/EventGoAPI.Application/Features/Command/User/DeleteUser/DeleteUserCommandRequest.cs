using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.User.DeleteUser
{
    public class DeleteUserCommandRequest : IRequest<DeleteUserCommandResponse>
    {
        public Guid Id { get; set; }
    }
}
