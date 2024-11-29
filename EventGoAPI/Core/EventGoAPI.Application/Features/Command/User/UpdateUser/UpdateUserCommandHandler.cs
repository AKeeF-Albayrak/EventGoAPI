using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Application.Enums;
using EventGoAPI.Application.Features.Command.Participant.CreateParticipant;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.User.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommandRequest, UpdateUserCommandResponse>
    {
        private IUserWriteRepository _userWriteRepository;
        private IUserReadRepository _userReadRepository;
        private IHttpContextAccessor _httpContextAccessor;
        public UpdateUserCommandHandler(IUserWriteRepository userWriteRepository, IUserReadRepository userReadRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userWriteRepository = userWriteRepository;
            _userReadRepository = userReadRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<UpdateUserCommandResponse> Handle(UpdateUserCommandRequest request, CancellationToken cancellationToken)
        {
            if (_httpContextAccessor.HttpContext?.Items["UserId"] is not Guid userId)
            {
                return new UpdateUserCommandResponse
                {
                    Success = false,
                    Message = "User ID could not be found or is not a valid GUID.",
                    ResponseType = ResponseType.Unauthorized
                };
            }

            var user = await _userReadRepository.GetEntityByIdAsync(userId);

            if (user == null)
            {
                return new UpdateUserCommandResponse
                {
                    Success = false,
                    Message = "User Cannot Be Found!",
                    ResponseType = ResponseType.Unauthorized
                };
            }

            if (request.Username != null) user.Username = request.Username;
            if (request.PasswordHash != null) user.PasswordHash = request.PasswordHash;
            if (request.Email != null) user.Email = request.Email;
            if (request.Address != null) user.Address = request.Address;
            if (request.City != null) user.City = request.City;
            if (request.Country != null) user.Country = request.Country;
            if (request.Latitude != null) user.Latitude = request.Latitude;
            if (request.Longitude != null) user.Longitude = request.Longitude;
            if (request.Interests != null) user.Interests = request.Interests;
            if (request.Name != null) user.Name = request.Name;
            if (request.Surname != null) user.Surname = request.Surname;
            if (request.PhoneNumber != null) user.PhoneNumber = request.PhoneNumber;
            if (request.Image != null) user.Image = request.Image;

            await _userWriteRepository.UpdateAsync(user);
            await _userWriteRepository.SaveChangesAsync();

            return new UpdateUserCommandResponse
            {
                Success = true,
                Message = "User Successfully Updated!",
                ResponseType = ResponseType.Success,
            };
        }
    }
}
