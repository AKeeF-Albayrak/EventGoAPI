using EventGoAPI.Application.Abstractions.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Query.Event.GetApprovedEvents
{
    public class GetApprovedEventQueryHandler : IRequestHandler<GetApprovedEventQueryRequest, GetApprovedEventQueryResponse>
    {
        private IEventReadRepository _eventReadRepository;
        private IUserReadRepository _userReadRepository;
        private IHttpContextAccessor _httpContextAccessor;

        public GetApprovedEventQueryHandler(IEventReadRepository eventReadRepository, IUserReadRepository userReadRepository, IHttpContextAccessor httpContextAccessor)
        {
            _eventReadRepository = eventReadRepository;
            _userReadRepository = userReadRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<GetApprovedEventQueryResponse> Handle(GetApprovedEventQueryRequest request, CancellationToken cancellationToken)
        {
            if (_httpContextAccessor.HttpContext?.Items["UserId"] is not Guid userId)
            {
                return new GetApprovedEventQueryResponse
                {
                    Message = "Invalid ID",
                    ResponseType = Enums.ResponseType.NotFound,
                };
            }
            var user = await _userReadRepository.GetEntityByIdAsync(userId);
            var events = await _eventReadRepository.GetAllEventsForUserAsync(userId);

            var scoredEvents = await Task.WhenAll(events.Select(async e => new
            {
                Event = e,
                Score = await CalculateEventScore(e, user)
            }));

            var recommendedEvents = scoredEvents
                .OrderByDescending(x => x.Score)
                .Select(x => x.Event)
                .ToList();

            return new GetApprovedEventQueryResponse
            {
                Events = recommendedEvents,
                Message = "Successfully!",
                ResponseType = Enums.ResponseType.Success,
            };
        }


        private async Task<int> CalculateEventScore(Domain.Entities.Event e, Domain.Entities.User user)
        {
            int score = 0;

            if (user.Interests != null && user.Interests.Contains(e.Category))
            {
                score += 10;
            }

            var userPastEvents = await _eventReadRepository.GetUserPastEventsAsync(user.Id);
            if (userPastEvents.Any(pastEvent => pastEvent.Category == e.Category))
            {
                score += 5;
            }

            double distance = CalculateDistance(user.Latitude, user.Longitude, e.Latitude, e.Longitude);
            if (distance < 10)
            {
                score += 10;
            }
            else if (distance < 50)
            {
                score += 5;
            }

            return score;
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371;

            var latDistance = ToRadians(lat2 - lat1);
            var lonDistance = ToRadians(lon2 - lon1);

            var a = Math.Sin(latDistance / 2) * Math.Sin(latDistance / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(lonDistance / 2) * Math.Sin(lonDistance / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c;
        }

        private double ToRadians(double angle)
        {
            return angle * (Math.PI / 180);
        }
    }
}
