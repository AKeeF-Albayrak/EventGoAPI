using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Application.Enums;
using EventGoAPI.Application.Features.Query.Event.GetAllEvents;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Query.User.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQueryRequest, GetUsersQueryResponse>
    {
        private IUserReadRepository _userReadRepository;
        public GetUsersQueryHandler(IUserReadRepository userReadRepository)
        {
            _userReadRepository = userReadRepository;
        }

        public async Task<GetUsersQueryResponse> Handle(GetUsersQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var users = await _userReadRepository.GetAllAsync();

                if (users == null || !users.Any())
                {
                    return new GetUsersQueryResponse
                    {
                        Success = false,
                        Message = "No users found.",
                        ResponseType = ResponseType.NotFound,
                        Users = Enumerable.Empty<Domain.Entities.User>(),
                        TotalUserCount = 0
                    };
                }

                return new GetUsersQueryResponse
                {
                    Success = true,
                    Message = "Users retrieved successfully.",
                    ResponseType = ResponseType.Success,
                    Users = users,
                    TotalUserCount = users.Count()
                };
            }
            catch (Exception ex)
            {
                return new GetUsersQueryResponse
                {
                    Success = false,
                    Message = "An error occurred while retrieving users.",
                    ResponseType = ResponseType.ServerError,
                    Users = Enumerable.Empty<Domain.Entities.User>(),
                    TotalUserCount = 0
                };
            }
        }
    }
}
