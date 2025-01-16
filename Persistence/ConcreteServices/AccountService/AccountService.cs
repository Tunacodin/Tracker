using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.DTOs;
using Application.DTOs.AccountDTOs;
using Application.DTOs.AuthDTOs;
using Application.Repositories.IAccountRepositories;
using Application.Repositories.ICustomerRepositories;
using Application.Services;
using Application.Utilities.Constants;
using Application.Utilities.Response;
using Application.VMs;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.HelperClass;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Persistence.Repositories.AccountRepository;
using QRCoder;

namespace Persistence.ConcreteServices.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly IAccountReadRepository readRepository;
        private readonly IAccountWriteRepository writeRepository;
        private readonly IMapper mapper;
        private readonly UserManager<Customer> userManager;
        private readonly IApiService apiService;
        private readonly IHttpContextAccessor httpContext;
        private readonly ICustomerReadRepository customerReadRepository;
        private readonly ICustomerWriteRepository customerWriteRepository;

        public AccountService(
            IAccountReadRepository readRepository,
            IAccountWriteRepository writeRepository,
            IMapper mapper,
            UserManager<Customer> userManager,
            IApiService apiService,
            IHttpContextAccessor httpContext,
            ICustomerReadRepository customerReadRepository,
            ICustomerWriteRepository customerWriteRepository
        )
        {
            this.readRepository = readRepository;
            this.writeRepository = writeRepository;
            this.mapper = mapper;
            this.userManager = userManager;
            this.apiService = apiService;
            this.httpContext = httpContext;
            this.customerReadRepository = customerReadRepository;
            this.customerWriteRepository = customerWriteRepository;
        }

        public async Task<GenericResponse<bool>> AddAccountToCustomerAsync(
            AddAccounToCustomerDTO model
        )
        {
            var account = await readRepository.GetSingleAsync(a => a.Phone == model.Phone);
            Customer? customer = await GetCustomerAsync(model.CustomerId);

            GenericResponse<bool> response = new();
            if (customer != null)
            {
                if (account == null)
                {
                    model.MobileDevice = Device.GetDevice();
                    var accountResult = await LoginAccountAsync(model);
                    if (!accountResult.IsSuccess)
                    {
                        response.IsSuccess = false;
                        response.Message = accountResult.Message;
                        return response;
                    }
                    Account newAccount = mapper.Map<Account>(model);
                    //otp gelince cikarilir bu listeden
                    newAccount.NeedToVerify.Add(customer.Id);
                    newAccount.Customer.Add(customer);
                    newAccount.DeviceId = model.MobileDevice.DeviceId;
                    newAccount.DeviceModel = model.MobileDevice.DeviceModel;
                    newAccount.WhoAdded = httpContext.HttpContext.Items["Email"] as string;
                    await writeRepository.AddAsync(newAccount);
                }
                else
                {
                    model.MobileDevice = account.GetAccountDevice();
                    var loginResult = await LoginAccountAsync(model);
                    if (!loginResult.IsSuccess) //yani sifre yanlis veya biryerde hata var
                    {
                        response.IsSuccess = false;
                        response.Message = loginResult.Message;
                        return response;
                    }
                    //burada success durumu  olur artik ya otp istenir bir daha yada token donmustur
                    var accounts = await readRepository.GetAccountsByCustomerIdAsync(customer.Id);
                    response.Data = true; //  bu data frontend icin verildi.buna gore logic isliyor
                    if (accounts.Any(a => a.Id == account.Id))
                    {
                        response.Message = Messages.LoginSucceeded;
                        if (loginResult.Data == "-97") //yani otp gerekliyse frontend false istiyor.islem sorunsuz olsaydi true isteyecekti
                        {
                            response.Data = false;
                        }
                        return response;
                    }
                    account.Customer.Add(customer);
                }

                bool result = await writeRepository.SaveAsync() > 0;
                if (result)
                    response.Message = Messages.AddSucceeded;
                else
                {
                    response.IsSuccess = false;
                    response.Message = Messages.SaveFail;
                }
            }
            else
            {
                response.IsSuccess = false;
                response.Message = Messages.NotExist;
            }
            return response;
        }

        /// <summary>
        /// Hard Delete
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<GenericResponse<bool>> DeleteAccountAsync(DeleteAccountDTO model)
        {
            var account = await readRepository.GetSingleAsync(a => a.Phone == model.Phone);
            GenericResponse<bool> response = new();
            if (account != null)
            {
                var result = writeRepository.Delete(account);
                await writeRepository.SaveAsync();
                if (result)
                    response.Message = Messages.DeleteSucceeded;
                else
                {
                    response.IsSuccess = false;
                    response.Message = Messages.Fail;
                }
            }
            else
            {
                response.IsSuccess = false;
                response.Message = Messages.NotExist;
            }

            return response;
        }

        public async Task<GenericResponse<bool>> RemoveAccountFromCustomerAsync(
            RemoveAccountFromCustomerDTO model
        )
        {
            GenericResponse<bool> response = new();
            var account = await readRepository.GetAccountByPhone(model.Phone);
            var customer = await customerReadRepository.GetCustomer(model.CustomerId);
            if (account == null || customer == null)
            {
                response.IsSuccess = false;
                response.Message = Messages.NotExist;
                return response;
            }
            try
            {
                if (account.Customer?.Any(c => c.Id == customer.Id) != true)
                {
                    response.IsSuccess = false;
                    response.Message = "Customer is not associated with this account";
                    return response;
                }

                // Remove the relationships from both sides
                account.Customer = account.Customer.Where(c => c.Id != customer.Id).ToList();
                customer.Accounts = customer.Accounts.Where(a => a.Phone != account.Phone).ToList();

                // Save changes
                var result = await writeRepository.SaveAsync() > 0;

                if (result)
                {
                    response.Message = Messages.DeleteSucceeded;
                    response.Data = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = Messages.Fail;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error removing account: {ex.Message}";
            }

            return response;
        }

        public async Task<GenericResponse<bool>> RemoveAccountAsync(DeleteAccountDTO model)
        {
            var account = await readRepository.GetSingleAsync(a => a.Phone == model.Phone);
            GenericResponse<bool> response = new();
            if (account != null)
            {
                var result = writeRepository.Remove(account);
                await writeRepository.SaveAsync();
                if (result)
                    response.Message = Messages.DeleteSucceeded;
                else
                {
                    response.IsSuccess = false;
                    response.Message = Messages.Fail;
                }
            }
            else
            {
                response.IsSuccess = false;
                response.Message = Messages.NotExist;
            }

            return response;
        }

        public async Task<GenericResponse<bool>> ActivateAccountAsync(DeleteAccountDTO model)
        {
            //var account = await readRepository.GetSingleAsync(a => a.Phone == model.Phone);
            var account = await readRepository.GetAccountByPhoneForAdmin(model.Phone);
            GenericResponse<bool> response = new();
            if (account != null)
            {
                var result = writeRepository.Update(account);
                await writeRepository.SaveAsync();
                if (result)
                    response.Message = Messages.UpdateSucceeded;
                else
                {
                    response.IsSuccess = false;
                    response.Message = Messages.Fail;
                }
            }
            else
            {
                response.IsSuccess = false;
                response.Message = Messages.NotExist;
            }

            return response;
        }

        public async Task<GenericResponse<string>> LoginAccountAsync(AccountLoginDTO model)
        {
            GenericResponse<string> response = new();
            var payload = new
            {
                SignIn = new
                {
                    MobilePhone = model.Phone,
                    Password = model.Password,
                    fastLogin = "",
                },
                MobileDevice = model.MobileDevice,
            };

            var apiResponse = await apiService.PostAsync<LoginData>("signin", payload);
            if (apiResponse is null)
            {
                response.IsSuccess = false;
                response.Message = Messages.Fail;
                return response;
            }
            var account = await readRepository.GetSingleAsync(a => a.Phone == model.Phone);
            if (apiResponse.Data == null || apiResponse.Response.ResultCode != 1)
            {
                // otp isteniyorsa 97 kodu donuyor.97den farkli gelirse burada isSucces false ataniyor cunku Frontend bu isSucces degerine gore calisiyor
                //isSucces default true gelir genericresponstan
                if (apiResponse.Response.ResultCode != -97)
                {
                    response.IsSuccess = false;
                    response.Message = apiResponse.Response.ResultMessage;
                    return response;
                }
                if (account != null) //Bizde var olan bir hesap ile login islemi yapiliyorsa ve OTP isteniyorsa sifreyi guncellemek gerekiyor
                {
                    account.AccountName = string.IsNullOrEmpty(model.AccountName)
                        ? account.AccountName
                        : model.AccountName;
                    account.Password = model.Password;

                    await writeRepository.SaveAsync();
                }
                response.Message = apiResponse.Response.ResultMessage;
                response.Data = "-97";
                return response;
            }
            account.Password = model.Password; //standart login sifre guncellemesi
            account.AccountName = string.IsNullOrEmpty(model.AccountName)
                ? account.AccountName
                : model.AccountName;
            account.Token = apiResponse.Data.TokenCode;
            await writeRepository.SaveAsync();
            response.Message = Messages.LoginSucceeded;
            response.Data = apiResponse.Data.TokenCode;
            return response;
        }

        public async Task<GenericResponse<string>> VerifyOTPAsync(CodeDTO model)
        {
            Customer? customer = await GetCustomerAsync(model.CustomerId);
            Account? account = await readRepository.GetSingleAsync(a => a.Phone == model.Phone);
            GenericResponse<string> response = new();
            if (customer == null || account == null)
            {
                response.IsSuccess = false;
                response.Message = Messages.NotExist;
                return response;
            }
            var payload = new
            {
                SignInOTP = new
                {
                    MobilePhone = model.Phone,
                    Password = model.Password,
                    OTP = model.Otp,
                    fcmToken = "",
                    fastLogin = "",
                },
                MobileDevice = account.GetAccountDevice(),
            };

            var apiResponse = await apiService.PostAsync<LoginData>("signinotp", payload);
            if (apiResponse is null)
            {
                response.IsSuccess = false;
                response.Message = Messages.Fail;
                return response;
            }
            if (apiResponse.Data == null || apiResponse.Response.ResultCode != 1)
            {
                response.IsSuccess = false;
                response.Message = apiResponse.Response.ResultMessage;

                return response;
            }
            account.Token = apiResponse.Data.TokenCode;
            account.NeedToVerify = account.NeedToVerify.Where(id => id != customer.Id).ToList();
            bool result = await writeRepository.SaveAsync() > 0;
            response.Message = Messages.LoginSucceeded;
            response.Data = apiResponse.Data.TokenCode;
            return response;
        }

        public async Task<GenericResponse<AccountVM>> GetBalance(GetBalanceDTO model)
        {
            GenericResponse<AccountVM> response = new();
            var user = await userManager.FindByEmailAsync(
                httpContext.HttpContext.Items["Email"] as string
            );
            var isAdmin = await userManager.IsInRoleAsync(user, Roles.Master);
            Account? account = null;
            if (!isAdmin)
            {
                account = await readRepository.GetByIdAsync(model.AccountId);
            }
            else
            {
                account = await readRepository.GetAccountByIdForAdmin(model.AccountId);
            }
            if (account == null)
            {
                response.IsSuccess = false;
                response.Message = Messages.NotExist;
                return response;
            }
            var payload = new { MobileDevice = account.GetAccountDevice() };

            var apiResponse = await apiService.PostAsync<BalanceData>(
                "getuserbalance",
                payload,
                account.Token
            );
            if (apiResponse is null)
            {
                response.IsSuccess = false;
                response.Message = Messages.Fail;
                return response;
            }

            if (apiResponse.Response.ResultCode == -146) //oturumun suresı dolduysa ve baska yerden girildiyse
            {
                var result = await LoginAccountAsync(
                    new AccountLoginDTO()
                    {
                        Phone = account.Phone,
                        Password = account.Password,
                        MobileDevice = account.GetAccountDevice(),
                    }
                );
                if (!result.IsSuccess)
                {
                    response.IsSuccess = false;
                    response.Message = result.Message;
                    return response;
                }
                apiResponse = await apiService.PostAsync<BalanceData>(
                    "getuserbalance",
                    payload,
                    account.Token
                );
            }
            else if (apiResponse.Data == null || apiResponse.Response.ResultCode != 1)
            {
                response.IsSuccess = false;
                response.Message = apiResponse.Response.ResultMessage;
                response.Data = mapper.Map<AccountVM>(account);
                response.Data.AccountName = response.Data.AccountName ?? "The name was not given";
                response.Data.Data = apiResponse.Response.ResultMessage;

                return response;
            }
            if (apiResponse is null)
            {
                response.IsSuccess = false;
                response.Message = Messages.Fail;
                return response;
            }
            if (apiResponse?.Data?.Balance == null)
            {
                response.IsSuccess = false;
                response.Message = Messages.Fail;
                response.Data = new AccountVM()
                {
                    AccountName = account.AccountName ?? "The name was not given",
                    Phone = account.Phone,
                    Data = "0", // or handle this case as needed
                };
                return response;
            }
            response.Data = mapper.Map<AccountVM>(account);
            response.Data.AccountName = response.Data.AccountName ?? "The name was not given";
            response.Data.Data = apiResponse.Data.Balance.ToString();
            if (
                string.IsNullOrEmpty(response.Data.MypayzNo)
                || string.IsNullOrEmpty(response.Data.Iban)
            )
            {
                var accountInfoResponse = await AccountInfo(
                    new AccountInfoDTO() { Phone = account.Phone }
                );
                if (accountInfoResponse.IsSuccess)
                {
                    response.Data.MypayzNo = accountInfoResponse.Data.MypayzNo;
                    response.Data.Iban = accountInfoResponse.Data.Iban;
                }
            }
            var transactionHistoryResponse = await GetTransactionsAsync(
                new AccountTransactionDTO()
                {
                    Phone = account.Phone,
                    FirstDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).ToString(
                        "yyyy-MM-dd"
                    ),
                    LastDate = new DateTime(
                        DateTime.Today.Year,
                        DateTime.Today.Month,
                        DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month)
                    ).ToString("yyyy-MM-dd"),
                }
            );
            if (transactionHistoryResponse.IsSuccess)
            {
                response.Data.MonthlyTransferCount = transactionHistoryResponse
                    .Data.TransactionList.Where(a => a.ProcessName == "Para Transferi")
                    .Count()
                    .ToString();
                response.Data.MonthlyLimit = transactionHistoryResponse
                    .Data.TransactionList.Where(a => a.ProcessName == "Para Transferi")
                    .Sum(a => a.Amount)
                    .ToString();
                response.Data.DailyLimit = transactionHistoryResponse
                    .Data.TransactionList.Where(a =>
                        a.ProcessName == "Para Transferi"
                        && DateTime.Parse(SwapDayAndMonth(a.Date)).Date == DateTime.Today.Date
                    )
                    .Sum(a => a.Amount)
                    .ToString();
            }
            return response;
        }

        public async Task<GenericResponse<AccountInfoVM>> AccountInfo(AccountInfoDTO model)
        {
            GenericResponse<AccountInfoVM> response = new();
            var account = await readRepository.GetSingleAsync(a => a.Phone == model.Phone);
            if (account == null)
            {
                response.IsSuccess = false;
                response.Message = Messages.NotExist;
                return response;
            }
            var payload = new { MobileDevice = account.GetAccountDevice() };

            var apiResponse = await apiService.PostAsync<AccountInfoData>(
                "profileview",
                payload,
                account.Token
            );
            if (apiResponse is null)
            {
                response.IsSuccess = false;
                response.Message = Messages.Fail;
                return response;
            }
            if (apiResponse.Data == null || apiResponse.Response.ResultCode != 1)
            {
                response.IsSuccess = false;
                response.Message = apiResponse.Response.ResultMessage;
                //response.Data = new AccountVM() { Phone = account.Phone, Data = apiResponse.Response.ResultMessage };
                return response;
            }
            if (
                !string.IsNullOrEmpty(apiResponse.Data.AccountNo)
                && !string.IsNullOrEmpty(apiResponse.Data.VirtualIban)
            )
            {
                response.IsSuccess = true;
                response.Data = new AccountInfoVM
                {
                    Phone = account.Phone,
                    MypayzNo = apiResponse.Data.AccountNo,
                    Iban = apiResponse.Data.VirtualIban,
                    Receiver = "MYPAYZ ÖDEME KURULUŞU A.Ş",
                };
                account.MypayzNo = apiResponse.Data.AccountNo;
                account.Iban = apiResponse.Data.VirtualIban;

                writeRepository.Update(account);
                await writeRepository.SaveAsync();
            }
            else
            {
                response.IsSuccess = false;
                response.Message = Messages.Fail;
            }
            return response;
        }

        public async Task<GenericResponse<IbanListData>> GetIbanList(AccountInfoDTO model)
        {
            GenericResponse<IbanListData> response = new();
            var account = await readRepository.GetAccountByPhone(model.Phone);
            if (account == null)
            {
                response.IsSuccess = false;
                response.Message = Messages.NotExist;
                return response;
            }
            var payload = new { ownAccount = "1", MobileDevice = account.GetAccountDevice() };

            var apiResponse = await apiService.PostAsync<IbanListData>(
                "getibanlist",
                payload,
                account.Token!
            );
            if (apiResponse is null)
            {
                response.IsSuccess = false;
                response.Message = Messages.Fail;
                return response;
            }
            if (apiResponse.Data == null || apiResponse.Response.ResultCode != 1)
            {
                response.IsSuccess = false;
                response.Message = apiResponse.Response.ResultMessage;
                return response;
            }
            response.Data = apiResponse.Data;
            return response;
        }

        public async Task<GenericResponse<TransactionHistoryData>> GetTransactionsAsync(
            AccountTransactionDTO model
        )
        {
            GenericResponse<TransactionHistoryData> response = new();
            var account = await readRepository.GetAccountByPhone(model.Phone);
            if (account == null)
            {
                response.IsSuccess = false;
                response.Message = Messages.NotExist;
                return response;
            }
            if (string.IsNullOrEmpty(model.LastDate))
                model.LastDate = DateTime.Today.ToString("yyyy-MM-dd");
            if (string.IsNullOrEmpty(model.FirstDate))
                model.FirstDate = DateTime.Today.AddDays(-30).ToString("yyyy-MM-dd");
            var transactionPayload = new
            {
                TransactionReports = new
                {
                    Count = model.Count > 0 ? model.Count : default(int?),
                    FirstDate = model.FirstDate,
                    LastDate = model.LastDate,
                },
                MobileDevice = account.GetAccountDevice(),
            };

            var apiResponse = await apiService.PostAsync<TransactionHistoryData>(
                "GetTransactionHistory",
                transactionPayload,
                account.Token!
            );
            if (apiResponse is null)
            {
                response.IsSuccess = false;
                response.Message = Messages.Fail;
                return response;
            }
            else if (apiResponse.Data == null || apiResponse.Response.ResultCode != 1)
            {
                response.IsSuccess = false;
                response.Message = apiResponse.Response.ResultMessage;
                return response;
            }

            response.IsSuccess = true;
            response.Data = apiResponse.Data;
            return response;
        }

        public async Task<GenericResponse<SendMoneyResponseData>> SendMoneyAsync(SendMoneyDTO model)
        {
            GenericResponse<SendMoneyResponseData> response = new();
            var account = await readRepository.GetAccountByPhone(model.Phone);
            if (account == null)
            {
                response.IsSuccess = false;
                response.Message = Messages.NotExist;
                return response;
            }
            var payload = new
            {
                MoneyTransfer = new
                {
                    IBANNo = model.IBAN,
                    Amount = model.Amount.ToString(),
                    NameSurname = model.NameSurname,
                    Description = model.Description,
                    Type = 2,
                    TransferType = TransferType.Individual,
                },
                MobileDevice = account.GetAccountDevice(),
            };

            var apiResponse = await apiService.PostAsync<SendMoneyResponseData>(
                "moneytransfertobankaccount",
                payload,
                account.Token!
            );
            if (apiResponse is null)
            {
                response.IsSuccess = false;
                response.Message = Messages.Fail;
                return response;
            }

            if (apiResponse.Response.ResultCode == -196)
            {
                response.IsSuccess = false;
                response.Message = "Invalid IBAN";
                return response;
            }
            else if (apiResponse.Data == null || apiResponse.Response.ResultCode != 1)
            {
                response.IsSuccess = false;
                response.Message = apiResponse.Response.ResultMessage;
                return response;
            }
            response.Data = apiResponse.Data;
            return response;
        }

        public async Task<GenericResponse<SendMoneyResponseData>> SendMoneyToMypayzAsync(
            SendMoneyToMypayzDTO model
        )
        {
            GenericResponse<SendMoneyResponseData> response = new();
            var account = await readRepository.GetAccountByPhone(model.Phone);
            if (account == null)
            {
                response.IsSuccess = false;
                response.Message = Messages.NotExist;
                return response;
            }
            var payload = new
            {
                InternalTransfer = new
                {
                    Receiver = model.ReceiverMypayzNumber,
                    Amount = model.Amount.ToString(),
                    TransferType = TransferType.Individual,
                    Comment = model.Description ?? string.Empty,
                },
                MobileDevice = account.GetAccountDevice(),
            };

            var apiResponse = await apiService.PostAsync<SendMoneyResponseData>(
                "internaltransfer",
                payload,
                account.Token!
            );

            if (apiResponse is null)
            {
                response.IsSuccess = false;
                response.Message = Messages.Fail;
                return response;
            }
            else if (apiResponse.Data == null || apiResponse.Response.ResultCode != 1)
            {
                response.IsSuccess = false;
                response.Message = apiResponse.Response.ResultMessage;
                return response;
            }
            response.Data = apiResponse.Data;
            return response;
        }

        public async Task<GenericResponse<VerifyMyPayzNumberData>> VerifyMypayzNumberAsync(
            VerifyMypayzNumberDTO model
        )
        {
            GenericResponse<VerifyMyPayzNumberData> response = new();
            var account = await readRepository.GetAccountByPhone(model.Phone);
            if (account == null)
            {
                response.IsSuccess = false;
                response.Message = Messages.NotExist;
                return response;
            }
            var payload = new
            {
                ReceiverVerify = new { ReceiverAccountNo = model.ReceiverMypayzNumber },
                MobileDevice = account.GetAccountDevice(),
            };

            var apiResponse = await apiService.PostAsync<VerifyMyPayzNumberData>(
                "GetReceiverVerify",
                payload,
                account.Token!
            );

            if (apiResponse is null)
            {
                response.IsSuccess = false;
                response.Message = Messages.Fail;
                return response;
            }
            else if (apiResponse.Data == null || apiResponse.Response.ResultCode != 1)
            {
                response.IsSuccess = false;
                response.Message = apiResponse.Response.ResultMessage;
                return response;
            }

            response.IsSuccess = true;
            response.Data = apiResponse.Data;
            return response;
        }

        public async Task<GenericResponse<string>> RequestMoneyQrAsync(RequestMoneyQrDTO model)
        {
            GenericResponse<string> response = new();
            var account = await readRepository.GetAccountByPhone(model.Phone);
            if (account == null)
            {
                response.IsSuccess = false;
                response.Message = Messages.NotExist;
                return response;
            }
            var accountInfo = await AccountInfo(new AccountInfoDTO() { Phone = model.Phone });
            if (!accountInfo.IsSuccess)
            {
                response.IsSuccess = false;
                response.Message = accountInfo.Message;
                return response;
            }

            var payload = new { mypayzno = accountInfo.Data.MypayzNo, price = model.Amount };

            using (var generator = new QRCodeGenerator())
            {
                var jsonPayload = JsonSerializer.Serialize(payload);
                var qrData = generator.CreateQrCode(jsonPayload, QRCodeGenerator.ECCLevel.Q);

                using (var qrCode = new PngByteQRCode(qrData))
                {
                    var qrCodeBytes = qrCode.GetGraphic(20);

                    var qrCodeBase64 = Convert.ToBase64String(qrCodeBytes);
                    response.IsSuccess = true;
                    response.Data = $"data:image/png;base64,{qrCodeBase64}";
                    return response;
                }
            }
        }

        public async Task<GenericResponse<bool>> RequestMoneyAsync(RequestMoneyDTO model)
        {
            GenericResponse<bool> response = new();
            var account = await readRepository.GetAccountByPhone(model.Phone);
            if (account == null)
            {
                response.IsSuccess = false;
                response.Message = Messages.NotExist;
                return response;
            }
            var payload = new
            {
                MoneyRequest = new
                {
                    Receiver = model.FromWho,
                    Amount = model.Amount,
                    Comment = model.Comment ?? string.Empty,
                },
                MobileDevice = account.GetAccountDevice(),
            };

            var apiResponse = await apiService.PostAsync<object>(
                "newmoneyrequest",
                payload,
                account.Token!
            );

            if (apiResponse is null)
            {
                response.IsSuccess = false;
                response.Message = Messages.Fail;
                return response;
            }
            else if (apiResponse.Response.ResultCode == 1)
            {
                response.Message = apiResponse.Response.ResultMessage;
            }
            else if (apiResponse.Data == null || apiResponse.Response.ResultCode != 1)
            {
                response.IsSuccess = false;
                response.Message = apiResponse.Response.ResultMessage;
                return response;
            }
            response.Data = true;
            return response;
        }

        private async Task<Customer?> GetCustomerAsync(string? customerId)
        {
            if (string.IsNullOrEmpty(customerId))
                return await userManager.FindByEmailAsync(
                    httpContext.HttpContext.Items["Email"] as string
                );
            return await userManager.FindByIdAsync(customerId);
        }

        private static string SwapDayAndMonth(string date)
        {
            var parts = date.Split('.');
            if (parts.Length == 3)
            {
                return $"{parts[1]}/{parts[0]}/{parts[2]}";
            }
            throw new FormatException("Invalid date format!");
        }
    }
}
