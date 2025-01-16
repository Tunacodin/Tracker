using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tracker.Api;

namespace Tracker.Models
{
    public class SendMoneyVM
    {
        public string Phone { get; set; }
        public string IBAN { get; set; }
        public decimal Amount { get; set; }
        public string NameSurname { get; set; }
        public string? Description { get; set; }
        public List<AccountVM>? Accounts { get; set; }
    }

    public class SendMoneyWithMypayzVM
    {
        public string Phone { get; set; }
        public string ReceiverMypayzNumber { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public List<AccountVM>? Accounts { get; set; }
    }
}
