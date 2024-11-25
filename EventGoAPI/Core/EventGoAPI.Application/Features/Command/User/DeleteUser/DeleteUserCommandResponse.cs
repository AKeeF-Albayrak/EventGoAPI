using EventGoAPI.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.User.DeleteUser
{
    public class DeleteUserCommandResponse
    {
        public bool Success { get; set; }
        public ResponseType ResponseType { get; set; }
        public string Message { get; set; }   
    }
}
