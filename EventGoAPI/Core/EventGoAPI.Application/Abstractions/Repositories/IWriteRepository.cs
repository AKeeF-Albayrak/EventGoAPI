using EventGoAPI.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Abstractions.Repositories
{
    public interface IWriteRepository<T> : IRepository<T> where T : BaseEntity
    {
        Task AddAsync(T model);
        Task<int> SaveChangesAsync();
        Task DeleteAsync(Guid id);
        Task<T> UpdateAsync(T model);
    }
}
