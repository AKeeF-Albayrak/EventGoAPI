using EventGoAPI.Application.Abstractions.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.Feedback.ReadFeedback
{
    public class ReadFeedbackCommandHandler : IRequestHandler<ReadFeedbackCommandRequest, ReadFeedbackCommandResponse>
    {
        private IFeedbackWriteRepository _feedbackWriteRepository;
        private IFeedbackReadRepository _feedbackReadRepository;
        public ReadFeedbackCommandHandler(IFeedbackReadRepository feedbackReadRepository, IFeedbackWriteRepository feedbackWriteRepository)
        {
            _feedbackReadRepository = feedbackReadRepository;
            _feedbackWriteRepository = feedbackWriteRepository;
        }
        public async Task<ReadFeedbackCommandResponse> Handle(ReadFeedbackCommandRequest request, CancellationToken cancellationToken)
        {
            var feedback = await _feedbackReadRepository.GetEntityByIdAsync(request.FeedbackID);

            if (feedback == null)
            {
                return new ReadFeedbackCommandResponse()
                {
                    Success = false,
                    Message = "Invalid ID!",
                    ResponseType = Enums.ResponseType.NotFound
                };
            }

            feedback.IsRead = true;

            await _feedbackWriteRepository.UpdateAsync(feedback);
            await _feedbackWriteRepository.SaveChangesAsync();

            return new ReadFeedbackCommandResponse()
            {
                Success = true,
                Message = "Successfully Readed Feedback!",
                ResponseType = Enums.ResponseType.Success
            };
        }
    }
}
