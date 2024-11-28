using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.FakeData.CreateFakeMessage
{
    public class CreateFakeMessageCommandHandler : IRequestHandler<CreateFakeMessageCommandRequest, CreateFakeMessageCommandResponse>
    {
        private IMessageWriteRepository _messageWriteRepository;
        public CreateFakeMessageCommandHandler(IMessageWriteRepository messageWriteRepository)
        {
            _messageWriteRepository = messageWriteRepository;
        }
        public async Task<CreateFakeMessageCommandResponse> Handle(CreateFakeMessageCommandRequest request, CancellationToken cancellationToken)
        {
            var message = new Domain.Entities.Message()
            {
                Id = Guid.NewGuid(),
                EventId = request.EventId,
                SenderId = request.SenderId,
                Text = request.Message, 
                SendingTime = request.SendingTime,
            };

            await _messageWriteRepository.AddAsync(message);
            return new CreateFakeMessageCommandResponse()
            {
                Message = message,
            };
        }
    }
}
