﻿using EventGoAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EventGoAPI.Domain.Entities
{
    public class Notification : BaseEntity
    {
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date {  get; set; }
        [JsonIgnore]
        public User User { get; set; }
    }
}
