using EventGoAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.FakeData.CreateFakeMessage
{
    public class CreateFakeMessageCommandResponse
    {
        public Domain.Entities.Message Message { get; set; }
    }
}
