using EventGoAPI.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Query.Event.GetEventById
{
    public class GetEventByIdQueryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ResponseType RepsonseType { get; set; }
        public Domain.Entities.Event Event { get; set; }
        public bool Auth { get; set; }
    }
}
