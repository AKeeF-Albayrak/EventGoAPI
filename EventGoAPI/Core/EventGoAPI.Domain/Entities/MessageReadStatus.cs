using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Domain.Entities
{
    public class MessageReadStatus
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public int UserId { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadTime { get; set; }

        public Message Message { get; set; }
        public User User { get; set; }
    }
}
