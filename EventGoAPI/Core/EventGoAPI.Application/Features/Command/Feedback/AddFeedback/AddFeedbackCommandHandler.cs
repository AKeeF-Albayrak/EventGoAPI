using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Application.Enums;
using EventGoAPI.Application.Features.Command.Event.CreateEvent;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.Feedback.AddFeedback
{
    public class AddFeedbackCommandHandler : IRequestHandler<AddFeedbackCommandRequest, AddFeedbackCommandResponse>
    {
        private IFeedbackWriteRepository _feedbackWriteRepository;
        private IHttpContextAccessor _httpContextAccessor;

        public AddFeedbackCommandHandler(IFeedbackWriteRepository feedbackWriteRepository, IHttpContextAccessor httpContextAccessor)
        {
            _feedbackWriteRepository = feedbackWriteRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<AddFeedbackCommandResponse> Handle(AddFeedbackCommandRequest request, CancellationToken cancellationToken)
        {
            if (_httpContextAccessor.HttpContext?.Items["UserId"] is not Guid userId)
            {
                return new AddFeedbackCommandResponse
                {
                    Success = false,
                    Message = "Invalid Id",
                    ResponseType = ResponseType.ValidationError
                };
            }

            Domain.Entities.Feedback feedback = new Domain.Entities.Feedback()
            {
                Id = Guid.NewGuid(),
                UserID = userId,
                Message = request.Message,
                SendingDate = DateTime.Now,
                IsRead = false,
            };

            await _feedbackWriteRepository.AddAsync(feedback);
            await _feedbackWriteRepository.SaveChangesAsync();

            return new AddFeedbackCommandResponse
            {
                Success = true,
                Message = "Feedback successfully added!",
                ResponseType = ResponseType.Success,
                Feedback = feedback
            };
        }
    }
}
