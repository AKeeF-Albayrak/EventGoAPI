using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Abstractions.Repositories
{
    public interface IUserReadRepository : IReadRepository<User>
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> CheckUserByUsernameEmailPhoneNumberAsync(string username, string email, string phoneNumber);
        Task<User> CheckLoginCredentials(string username, string password); 
        Task<int> GetUserCountAsync();
    }
}
