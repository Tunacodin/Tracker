using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.VMs
{
    public class CustomerVM
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public DateTime CreationDate { get; set; }
        public string? WhoAdded { get; set; }
        public Status Status { get; set; }
    }

    public class TransferAccountToCustomerVM
    {
        public string Code { get; set; }
        public string CustomerId { get; set; }
    }
}
