using Microsoft.EntityFrameworkCore;
using EventGoAPI.Domain.Entities;
using EventGoAPI.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Persistence.Concretes.Repositories;

namespace EventGoAPI.Persistence.Concretes.Repositories
{
    public class UserReadRepository : ReadRepository<User>, IUserReadRepository
    {
        private readonly EventGoDbContext _context;
        public UserReadRepository(EventGoDbContext context) : base(context)
        {
            _context = context;
        }

        public DbSet<User> Table => _context.Set<User>();

        public async Task<User> CheckUserByUsernameEmailPhoneNumberAsync(string username, string email, string phoneNumber)
        {
            return await Table.FirstOrDefaultAsync(u => u.Username == username || u.Email == email || u.PhoneNumber == phoneNumber);
        }

        public async Task<User> GetUserByUsernameAsync(string username) => await Table.SingleOrDefaultAsync(u => u.Username == username);
    }
}
