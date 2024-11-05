using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Application.Abstractions.Services;
using EventGoAPI.Application.Dtos.AuthDtos;
using EventGoAPI.Domain.Entities;
using EventGoAPI.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;

namespace EventGoAPI.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IPasswordHasher _passwordHasher;
        public AuthController(IUserReadRepository userReadRepository, IUserWriteRepository userWriteRepository, IPasswordHasher passwordHasher)
        {
            _userWriteRepository = userWriteRepository;
            _userReadRepository = userReadRepository;
            _passwordHasher = passwordHasher;  
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userReadRepository.GetUserByUsernameAsync(dto.Username);
            if (user != null)
            {
                var hashedPasswordFromDb = user.PasswordHash;

                bool isPasswordValid = _passwordHasher.VerifyPassword(dto.Password, hashedPasswordFromDb);

                if (isPasswordValid)
                {
                    return Ok("Login successful");
                }
                else
                {
                    return Unauthorized("Invalid credentials");
                }
            }

            return NotFound("User not found");
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] SignUpDto dto)
        {
            var testUser = await _userReadRepository.CheckUserByUsernameEmailPhoneNumberAsync(dto.Username, dto.Email, dto.PhoneNumber);
            if (testUser != null)
            {
                return Unauthorized("Invalid credentials");
            }
            User user = new User
            {
                Id = Guid.NewGuid(),
                Role = Domain.Enums.UserRole.Normal,
                Username = dto.Username,
                PasswordHash = _passwordHasher.HashPassword(dto.PasswordHash),
                Email = dto.Email,
                Address = dto.Address,
                City = dto.City,
                Country = dto.Country,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Interests = dto.Interests,
                Name = dto.Name,
                Surname = dto.Surname,
                BirthDate = dto.BirthDate,
                Gender = dto.Gender,
                PhoneNumber = dto.PhoneNumber,
                //Image = dto.Image,
                CreatedTime = DateTime.UtcNow,
            };
            await _userWriteRepository.AddAsync(user);
            await _userWriteRepository.SaveChangesAsync();
            return Ok(user);
        }

        /*[HttpPost]
        public async Task<IActionResult> ForgotPassword()
        {

        }*/
    }
}
