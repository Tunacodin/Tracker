using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Customer : IdentityUser, IBaseEntity
    {
        public Customer()
        {
            Accounts = new List<Account>();
        }
        public string FullName { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public Status Status { get; set; } = Status.Active;
        public string? WhoAdded { get; set; }
        public List<Account> Accounts { get; set; }

        public string? VerificationCode { get; set; }
        public DateTime? VerificationCodeExpiration { get; set; }
    }
}
