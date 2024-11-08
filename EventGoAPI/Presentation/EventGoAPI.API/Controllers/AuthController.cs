using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Application.Abstractions.Services;
using EventGoAPI.Application.Dtos.AuthDtos;
using EventGoAPI.Domain.Entities;
using EventGoAPI.Persistence.Concretes.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using System.Security.Claims;

namespace EventGoAPI.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        public AuthController(IUserReadRepository userReadRepository, IUserWriteRepository userWriteRepository, IPasswordHasher passwordHasher, ITokenService tokenService, IEmailService emailService)
        {
            _userWriteRepository = userWriteRepository;
            _userReadRepository = userReadRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _emailService = emailService;
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
                    var token = _tokenService.GenerateToken(user);
                    return Ok(new { Token = token, Message = "Login successful" });
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

        [HttpPost]
        public async Task<IActionResult> SendEmail([FromBody] string email)
        {
            var random = new Random();
            string verificationCode = random.Next(100000, 999999).ToString();

            var user = await _userReadRepository.CheckUserByUsernameEmailPhoneNumberAsync("", email, "");
            if (user == null) return BadRequest("Email Not Found");
            await _emailService.SendEmailAsync(email, verificationCode);
            user.VerificationCode = verificationCode;
            user.VerificationCodeExpiration = DateTime.Now.AddMinutes(3);

            await _userWriteRepository.UpdateAsync(user);
            await _userWriteRepository.SaveChangesAsync();

            return Ok("Verification Code Sended!");
        }

        [HttpPost]
        public async Task<IActionResult> VerifyCode(string email, string enteredCode)
        {
            var user = await _userReadRepository.CheckUserByUsernameEmailPhoneNumberAsync("", email, "");

            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (user.VerificationCode == null || user.VerificationCodeExpiration == null)
            {
                return BadRequest("No verification code found. Please request a new code.");
            }

            if (user.VerificationCodeExpiration < DateTime.Now)
            {
                return BadRequest("The verification code has expired. Please request a new code.");
            }

            if (user.VerificationCode != enteredCode)
            {
                return BadRequest("Invalid verification code.");
            }

            user.VerificationCode = null;
            user.VerificationCodeExpiration = null;
            user.PasswordResetAuthorized = true;
            user.PasswordResetAuthorizedExpiration = DateTime.Now.AddMinutes(10);

            await _userWriteRepository.UpdateAsync(user);
            await _userWriteRepository.SaveChangesAsync();

            return Ok("Verification successful. You are now authenticated.");
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword([FromBody] string newPassword, string mail)
        {
            var user = await _userReadRepository.CheckUserByUsernameEmailPhoneNumberAsync("", mail, "");

            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (!user.PasswordResetAuthorized || user.PasswordResetAuthorizedExpiration < DateTime.Now)
            {
                user.PasswordResetAuthorized = false;
                user.PasswordResetAuthorizedExpiration = null;

                await _userWriteRepository.UpdateAsync(user);
                await _userWriteRepository.SaveChangesAsync();

                return Unauthorized("You are not authorized to update the password. Please verify the code again.");
            }

            user.PasswordHash = _passwordHasher.HashPassword(newPassword);
            user.PasswordResetAuthorized = false;
            user.PasswordResetAuthorizedExpiration = null;

            await _userWriteRepository.UpdateAsync(user);
            await _userWriteRepository.SaveChangesAsync();

            return Ok("Password updated successfully.");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is missing.");
            }

            await _tokenService.AddToBlacklistAsync(token);

            return Ok("Successfully logged out.");
        }
    }
}
