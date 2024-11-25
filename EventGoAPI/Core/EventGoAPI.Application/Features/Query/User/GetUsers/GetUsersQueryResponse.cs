using EventGoAPI.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Query.User.GetUsers
{
    public class GetUsersQueryResponse
    {
        public bool Success { get; set; }
        public IEnumerable<Domain.Entities.User> Users { get; set; }
        public string Message { get; set; }
        public ResponseType ResponseType { get; set; }
        public int TotalUserCount { get; set; }
    }
}
