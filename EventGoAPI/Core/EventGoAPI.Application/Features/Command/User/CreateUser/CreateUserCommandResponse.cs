using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.User.CreateUser
{
    public class CreateUserCommandResponse
    {
        public bool Success { get; set; }
        public Domain.Entities.User User { get; set; }
        public string Message { get; set; }
    }
}
