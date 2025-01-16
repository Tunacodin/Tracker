using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Utilities.Response;

namespace Application.VMs
{
    public class AccountVM
    {
        public string Phone { get; set; }
        public string Data { get; set; }
        public string AccountName { get; set; }
        public string? MonthlyLimit { get; set; }
        public string? MonthlyTransferCount { get; set; }
        public string? DailyLimit { get; set; }
        public string? MypayzNo { get; set; }
        public string? Iban { get; set; }
    }

    public class AccountInfoVM
    {
        public string Phone { get; set; }
        public string MypayzNo { get; set; }
        public string Iban { get; set; }
        public string Receiver { get; set; }
    }

    public class BalancesVM
    {
        public BalancesVM()
        {
            Accounts = new List<AccountVM>();
        }

        public List<AccountVM> Accounts { get; set; }
        public string CustomerId { get; set; }
    }

    public class BalancesVMPaginated : PaginatedResponse<AccountVM>
    {
        public BalancesVMPaginated()
        {
            Data = new List<AccountVM>();
        }

        public string CustomerId { get; set; }
    }
}
