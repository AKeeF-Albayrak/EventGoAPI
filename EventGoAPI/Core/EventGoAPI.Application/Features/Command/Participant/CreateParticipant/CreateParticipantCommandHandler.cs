using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.Participant.CreateParticipant
{
    public class CreateParticipantCommandHandler : IRequestHandler<CreateParticipantCommandRequest, CreateParticipantCommandResponse>
    {
        private IParticipantWriteRepository _participantWriteRepository;
        private IParticipantReadRepository _participantReadRepository;
        private IHttpContextAccessor _httpContextAccessor;

        public CreateParticipantCommandHandler(IParticipantReadRepository participantReadRepository, IParticipantWriteRepository participantWriteRepository, IHttpContextAccessor httpContextAccessor)
        {
            _participantReadRepository = participantReadRepository;
            _participantWriteRepository = participantWriteRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<CreateParticipantCommandResponse> Handle(CreateParticipantCommandRequest request, CancellationToken cancellationToken)
        {
            if (_httpContextAccessor.HttpContext?.Items["UserId"] is not Guid userId)
            {
                throw new UnauthorizedAccessException("User ID could not be found or is not a valid GUID.");
            }

            var participant = new Domain.Entities.Participant
            {
                Id = userId,
                EventId = request.EventId,
            };
            var test = await _participantReadRepository.GetEntityByIdAsync(userId.ToString(), request.EventId.ToString());
            if(test != null)
            {
                return new CreateParticipantCommandResponse()
                {
                    Succsess = false
                };
            }
            await _participantWriteRepository.AddAsync(participant);
            await _participantWriteRepository.SaveChangesAsync();

            return new CreateParticipantCommandResponse()
            {
                Succsess = true
            };
        }
    }
}
