using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tracker.Api;

namespace Tracker.Models
{
    public class GenerateCodeVM
    {
        public GenerateCodeVM()
        {
            Customers = new();
            Accounts = new();
        }

        public List<CustomerVM> Customers { get; set; }
        public List<AccountVM> Accounts { get; set; }
    }
}
