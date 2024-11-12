using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Application.Abstractions.Services;
using EventGoAPI.Application.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.User.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        private IUserReadRepository _userReadRepository;
        private IUserWriteRepository _userWriteRepository;
        private IPasswordHasher _passwordHasher;
        public CreateUserCommandHandler(IUserReadRepository userReadRepository, IUserWriteRepository userWriteRepository, IPasswordHasher passwordHasher)
        {
            _userReadRepository = userReadRepository;
            _userWriteRepository = userWriteRepository;
            _passwordHasher = passwordHasher;
        }
        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            var case1 = await _userReadRepository.CheckUserByUsernameEmailPhoneNumberAsync(request.Username, request.Email, request.PhoneNumber);

            if (case1 != null)
            {
                return new CreateUserCommandResponse
                {
                    Success = false,
                    Message = "A user with the same username, email, or phone number already exists.",
                    ResponseType = ResponseType.Conflict
                };
            }

            var user = new Domain.Entities.User
            {
                Id = Guid.NewGuid(),
                Role = Domain.Enums.UserRole.Normal,
                Username = request.Username,
                PasswordHash = _passwordHasher.HashPassword(request.PasswordHash),
                Email = request.Email,
                Address = request.Address,
                City = request.City,
                Country = request.Country,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Interests = request.Interests,
                Name = request.Name,
                Surname = request.Surname,
                BirthDate = request.BirthDate,
                Gender = request.Gender,
                PhoneNumber = request.PhoneNumber,
                CreatedTime = DateTime.Now,
                PasswordResetAuthorized = false,
            };

            await _userWriteRepository.AddAsync(user);
            await _userWriteRepository.SaveChangesAsync();

            return new CreateUserCommandResponse
            {
                Success = true,
                Message = "User Successfully Added",
                ResponseType = ResponseType.Success,
                User = user
            };
        }
    }
}
