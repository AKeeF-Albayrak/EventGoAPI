using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.FakeData.CreateFakeEvent
{
    public class CreateFakeEventCommandResponse
    {
        public Domain.Entities.Event Event { get; set; }
    }
}
