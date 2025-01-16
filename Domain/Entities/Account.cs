using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;
using Domain.HelperClass;
using Domain.Interfaces;

namespace Domain.Entities
{
    public class Account : IBaseEntity
    {
        public Account()
        {
            Customer = new List<Customer>();
            NeedToVerify = new List<string>();
        }

        public List<Customer> Customer { get; set; }
        public string Id { get; set; }
        public string? AccountName { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public Status Status { get; set; }
        public Membership Membership { get; set; }
        public string? Token { get; set; }
        public List<string> NeedToVerify { get; set; }
        public string? WhoAdded { get; set; }
        public string? DeviceId { get; set; }
        public string? DeviceModel { get; set; }
        public string? MypayzNo { get; set; }
        public string? Iban { get; set; }

        public MobileDevice GetAccountDevice()
        {
            return new MobileDevice { DeviceId = DeviceId, DeviceModel = DeviceModel };
        }
    }
}
