using EventGoAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Query.Message.GetEventMessages
{
    public class GetEventMessagesResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ICollection<Domain.Entities.Message> Messages { get; set; }
    }
}
