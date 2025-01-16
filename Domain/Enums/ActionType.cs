using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
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
