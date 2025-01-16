using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;
using Domain.Interfaces;

namespace Domain.Entities
{
    public class Code : IBaseEntity
    {
        public Code()
        {
            AccountPhoneNumbers = new List<string>();
        }

        public string Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public Status Status { get; set; }
        public string CustomerId { get; set; }
        public string? WhoUsed { get; set; }
        public List<string> AccountPhoneNumbers { get; set; }
    }
}
