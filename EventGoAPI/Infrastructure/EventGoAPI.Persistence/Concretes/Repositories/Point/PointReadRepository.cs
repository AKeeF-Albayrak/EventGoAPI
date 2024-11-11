using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Domain.Entities;
using EventGoAPI.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Persistence.Concretes.Repositories
{
    public class PointReadRepository : ReadRepository<Point>, IPointReadRepository
    {
        public PointReadRepository(EventGoDbContext context) : base(context)
        {
        }
    }
}
