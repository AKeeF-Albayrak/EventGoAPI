using EventGoAPI.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Query.User.LoginUser
{
    public class LoginUserQueryResponse
    {
        public bool Success { get; set; }
        public Domain.Entities.User User { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
        public ResponseType ResponseType { get; set; }
    }
}
