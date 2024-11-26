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

namespace EventGoAPI.Application.Features.Query.Point
{
    public class GetPointsQueryHandler : IRequestHandler<GetPointsQueryRequest , GetPointsQueryResponse>
    {
        private IPointReadRepository _pointReadRepository;
        private IHttpContextAccessor _httpContextAccessor;
        public GetPointsQueryHandler(IPointReadRepository pointReadRepository, IHttpContextAccessor httpContextAccessor)
        {
            _pointReadRepository = pointReadRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetPointsQueryResponse> Handle(GetPointsQueryRequest request, CancellationToken cancellationToken)
        {
            if (_httpContextAccessor.HttpContext?.Items["UserId"] is not Guid userId)
            {
                return new GetPointsQueryResponse
                {
                    Success = false,
                    Message = "Invalid Id",
                    ResponseType = ResponseType.ValidationError
                };
            }

            var points = await _pointReadRepository.GetPointsAsync(userId);

            var totalPoints = points.Sum(point => point.Score);

            return new GetPointsQueryResponse()
            {
                Success = true,
                Message = "Success!",
                ResponseType = ResponseType.Success,
                Points = points,
                Point = totalPoints
            };
        }
    }
}
