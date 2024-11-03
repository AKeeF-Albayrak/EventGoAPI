using EventGoAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Domain.Entities
{
    public class Point : BaseEntity
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public int Score { get; set; }
        public DateTime Date { get; set; }

        public User User { get; set; }
    }
}
