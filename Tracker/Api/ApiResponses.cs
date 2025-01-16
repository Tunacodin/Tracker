namespace Tracker.Api
{
    public enum Status
    {
        Active = 1,
        Modified,
        Pasive,
    }

    public class Token
    {
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
        public string Id { get; set; }
    }

    public class AccountVM
    {
        public string Phone { get; set; }
        public string AccountName { get; set; }
        public string Data { get; set; }
        public string? MypayzNo { get; set; }
        public string? Iban { get; set; }
        public string? MonthlyLimit { get; set; }
        public string? MonthlyTransferCount { get; set; }
        public string? DailyLimit { get; set; }
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

    public class AccountInfoVM
    {
        public string Phone { get; set; }
        public string MypayzNo { get; set; }
        public string Iban { get; set; }
        public string Receiver { get; set; }
    }

    public class IbanListData
    {
        public List<Iban> IbanList { get; set; }
    }

    public class Iban
    {
        public int IBANListId { get; set; }
        public string BankName { get; set; }
        public string IBANNo { get; set; }
        public string NameSurname { get; set; }
        public string Description { get; set; }
        public string BankLogo { get; set; }
    }

    public class TransactionHistoryData
    {
        public List<Transaction> TransactionList { get; set; }
    }

    public class Transaction
    {
        public int ProcessId { get; set; }
        public int ProcessType { get; set; }
        public string ProcessName { get; set; }
        public string ReceiverIBAN { get; set; }
        public string SenderName { get; set; }
        public decimal Amount { get; set; }
        public decimal Profit { get; set; }
        public string AvatarData { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
    }

    public class SendMoneyResponse
    {
        public long TransactionId { get; set; }
        public decimal TransactionAmount { get; set; }
    }

    public class CustomerVM
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public DateTime CreationDate { get; set; }
        public string? WhoAdded { get; set; }
        public Status? Status { get; set; }
    }

    public class RemoveAccountFromCustomerDTO
    {
        public string Phone { get; set; }
        public string CustomerId { get; set; }
    }

    public class DeleteAccountDTO
    {
        public string Phone { get; set; }
    }

    public class CodeVM
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
    }

    public class VerifyCodeDTO
    {
        public string CodeId { get; set; }
        public string CustomerEmail { get; set; }
    }

    public class PaginatedResponse<T>
    {
        public PaginatedResponse()
        {
            Data = new List<T>();
        }

        public List<T> Data { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;
    }

    public class BalancesVMPaginated : PaginatedResponse<AccountVM>
    {
        public BalancesVMPaginated()
        {
            Data = new List<AccountVM>();
        }

        public string CustomerId { get; set; }
    }

    public class LogVM
    {
        public string Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string Message { get; set; }
        public string? LoggedInUserEmail { get; set; }
        public string? TargetEmail { get; set; }
        public string? TargetAccount { get; set; }
        public string? IpAddress { get; set; }
        public ActionType? ActionType { get; set; }
        public string? Page { get; set; }
        public ProcessStatus? ProcessStatus { get; set; }
    }

    public enum ProcessStatus
    {
        Started = 1,
        Completed,
        Failed,
    }

    public enum ActionType
    {
        Login = 1,
        Logout,
        CreateCustomer,
        GetBalances,
        GetCustomerAccounts,
        GetAllCustomers,
        GetAllCustomersPaginated,
        TransferAccountToCustomer,
        VerifyCode,
        DeleteCustomer,
        MakeActiveCustomer,
        SearchCustomer,
        AddAccountToCustomer,
        RemoveAccountFromCustomer,
        RemoveAccount,
        ActivateAccount,
        LoginAccount,
        VerifyOTP,
        GetBalanceForAccount,
        AccountInfo,
        GetIbanList,
        GetTransactions,
        SendMoney,
        SendMoneyToMypayz,
        VerifyMypayzNumber,
        RequestMoneyQr,
        RequestMoney,
    }
}
