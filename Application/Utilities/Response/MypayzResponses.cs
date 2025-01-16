using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utilities.Response
{
    public class JsonResponse<T>
    {
        public Response Response { get; set; }
        public T Data { get; set; }
    }

    public class Response
    {
        public bool Result { get; set; }
        public int ResultCode { get; set; }
        public string ResultMessage { get; set; }
    }

    public class LoginData
    {
        public string TokenCode { get; set; }
        public string LastSuccessfulLoginDate { get; set; }
        public string LastFailedLoginDate { get; set; }
    }

    public class AccountInfoData
    {
        public string AccountNo { get; set; }
        public string VirtualIban { get; set; }
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

    public class BalanceData
    {
        public decimal Balance { get; set; }
    }

    public class IbanListData
    {
        public List<Iban> IbanList { get; set; }
    }

    public class Iban
    {
        //public string IbanNumber { get; set; }
        //public string AccountHolderName { get; set; }
        public int IBANListId { get; set; }
        public string BankName { get; set; }
        public string IBANNo { get; set; }
        public string NameSurname { get; set; }
        public string Description { get; set; }
        public string BankLogo { get; set; }
    }

    public class SendMoneyResponseData
    {
        public long TransactionId { get; set; }
        public decimal TransactionAmount { get; set; }
    }

    public class VerifyMyPayzNumberData
    {
        public string Name { get; set; }
    }
}
