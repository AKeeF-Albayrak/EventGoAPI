using EventGoAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EventGoAPI.Domain.Entities
{
    public class Feedback : BaseEntity
    {
        public string Message { get; set; }
        public DateTime SendingDate { get; set; }
        public bool IsRead { get; set; }    
        public Guid UserID { get; set; }

        [JsonIgnore]
        public User User { get; set; }
    }
}
