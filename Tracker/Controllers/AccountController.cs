using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tracker.Api;
using Tracker.Constants;
using Tracker.Helpers;
using Tracker.Models;

namespace Tracker.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApiService apiService;

        public AccountController(ApiService apiService)
        {
            this.apiService = apiService;
        }

        public async Task<IActionResult> AddAccountToCustomer(string phone)
        {
            ViewBag.Phone = phone;
            var isMaster = HttpContext.User.IsInRole(Roles.Master);
            if (isMaster)
            {
                var response = await apiService.GetAsync<List<CustomerVM>>("Customer");
                if (response != null && response.IsSuccess)
                {
                    ViewBag.Customers = response.Data;
                }
            }
            await apiService.PostAsync<bool>(
                "Log",
                new CreateMVCLogDTO { Page = MVCPages.AddAccount }
            );
            return View();
        }

        public async Task<IActionResult> Info(string phone)
        {
            if (string.IsNullOrEmpty(phone))
            {
                return RedirectToAction("Index", "Home");
            }
            var response = await apiService.GetAsync<AccountInfoVM>($"Account/Info?phone={phone}");
            if (response != null && response.IsSuccess)
            {
                await apiService.PostAsync<bool>(
                    "Log",
                    new CreateMVCLogDTO { Page = MVCPages.AccountInfo }
                );
                return View(response.Data);
            }
            TempData.SetNotification(
                response?.Message ?? "An error occurred while retrieving account information."
            );
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> IbanList(string phone)
        {
            if (string.IsNullOrEmpty(phone))
            {
                return RedirectToAction("Index", "Home");
            }
            var response = await apiService.GetAsync<IbanListData>(
                $"Account/Iban-List?phone={phone}"
            );
            if (response != null && response.IsSuccess)
            {
                await apiService.PostAsync<bool>(
                    "Log",
                    new CreateMVCLogDTO { Page = MVCPages.IbanList }
                );
                return View(response.Data);
            }
            TempData.SetNotification(
                response?.Message ?? "An error occurred while retrieving account iban list."
            );
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Transactions(
            string phone,
            string firstDate = "",
            string lastDate = "",
            int count = 0
        )
        {
            var filter = new TransactionFilterVM
            {
                Phone = phone,
                Count = count,
                FirstDate = firstDate,
                LastDate = lastDate,
            };

            var response = await apiService.PostAsync<TransactionHistoryData>(
                "Account/Transactions",
                filter
            );

            if (response != null && response.IsSuccess)
            {
                ViewBag.Filter = filter;
                await apiService.PostAsync<bool>(
                    "Log",
                    new CreateMVCLogDTO { Page = MVCPages.Transactions }
                );
                return View(response.Data);
            }
            TempData.SetNotification(
                response?.Message ?? "An error occurred while retrieving account transactions."
            );
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> SendMoney(string phone = "")
        {
            if (!string.IsNullOrEmpty(phone))
            {
                var sendMoneyVM = new SendMoneyVM
                {
                    Accounts = new List<AccountVM> { new AccountVM { Phone = phone } },
                };
                await apiService.PostAsync<bool>(
                    "Log",
                    new CreateMVCLogDTO { Page = MVCPages.SendMoney }
                );
                return View(sendMoneyVM);
            }
            else
            {
                var response = await apiService.GetAsync<List<AccountVM>>(
                    "Customer/Customer-Accounts"
                );
                if (response != null && response.IsSuccess)
                {
                    var sendMoneyVM = new SendMoneyVM { Accounts = response.Data };
                    await apiService.PostAsync<bool>(
                        "Log",
                        new CreateMVCLogDTO { Page = MVCPages.SendMoney }
                    );
                    return View(sendMoneyVM);
                }
                TempData.SetNotification(
                    response?.Message ?? "An error occurred while retrieving account information."
                );
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> SendMoney(SendMoneyVM model)
        {
            if (!ModelState.IsValid)
            {
                var accountsResponse = await apiService.GetAsync<List<AccountVM>>(
                    "Customer/Customer-Accounts"
                );
                if (accountsResponse != null && accountsResponse.IsSuccess)
                {
                    model.Accounts = accountsResponse.Data;
                }
                return View(model);
            }

            var response = await apiService.PostAsync<SendMoneyResponse>(
                "Account/Send-Money",
                model
            );

            if (response != null && response.IsSuccess)
            {
                TempData.SetNotification(
                    $"{response.Data.TransactionAmount} Money sent successfully.",
                    "Success"
                );
                if (HttpContext.User.IsInRole(Roles.Master))
                {
                    var email = HttpContext.Request.Query["email"].ToString();
                    if (!string.IsNullOrEmpty(email))
                    {
                        return RedirectToAction("Index", "Home", new { email = email });
                    }
                    return RedirectToAction("AdminDashboard", "Home");
                }
                return RedirectToAction("Index", "Home");
            }

            TempData.SetNotification(response?.Message ?? "An error occurred while sending money.");

            var refreshAccountsResponse = await apiService.GetAsync<List<AccountVM>>(
                "Customer/Customer-Accounts"
            );
            if (refreshAccountsResponse != null && refreshAccountsResponse.IsSuccess)
            {
                model.Accounts = refreshAccountsResponse.Data;
            }

            return View(model);
        }

        public async Task<IActionResult> SendMoneyWithMypayz(string phone = "")
        {
            if (!string.IsNullOrEmpty(phone))
            {
                var sendMoneyVM = new SendMoneyWithMypayzVM
                {
                    Accounts = new List<AccountVM> { new AccountVM { Phone = phone } },
                };
                await apiService.PostAsync<bool>(
                    "Log",
                    new CreateMVCLogDTO { Page = MVCPages.SendMoneyToMypayz }
                );
                return View(sendMoneyVM);
            }
            else
            {
                var response = await apiService.GetAsync<List<AccountVM>>(
                    "Customer/Customer-Accounts"
                );
                if (response != null && response.IsSuccess)
                {
                    var sendMoneyVM = new SendMoneyWithMypayzVM { Accounts = response.Data };
                    await apiService.PostAsync<bool>(
                        "Log",
                        new CreateMVCLogDTO { Page = MVCPages.SendMoneyToMypayz }
                    );
                    return View(sendMoneyVM);
                }
                TempData.SetNotification(
                    response?.Message ?? "An error occurred while retrieving account information."
                );
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> SendMoneyWithMypayz(SendMoneyWithMypayzVM model)
        {
            if (!ModelState.IsValid)
            {
                var accountsResponse = await apiService.GetAsync<List<AccountVM>>(
                    "Customer/Customer-Accounts"
                );
                if (accountsResponse != null && accountsResponse.IsSuccess)
                {
                    model.Accounts = accountsResponse.Data;
                }
                return View(model);
            }

            var response = await apiService.PostAsync<SendMoneyResponse>(
                "Account/Send-Money-To-Mypayz",
                model
            );

            if (response != null && response.IsSuccess)
            {
                TempData.SetNotification(
                    $"{response.Data.TransactionAmount} Money sent successfully.",
                    "Success"
                );
                if (HttpContext.User.IsInRole(Roles.Master))
                {
                    return RedirectToAction("AdminDashboard", "Home");
                }
                return RedirectToAction("Index", "Home");
            }

            TempData.SetNotification(response?.Message ?? "An error occurred while sending money.");

            var refreshAccountsResponse = await apiService.GetAsync<List<AccountVM>>(
                "Customer/Customer-Accounts"
            );
            if (refreshAccountsResponse != null && refreshAccountsResponse.IsSuccess)
            {
                model.Accounts = refreshAccountsResponse.Data;
            }

            return View(model);
        }

        public async Task<IActionResult> RequestMoneyQr(string phone)
        {
            if (string.IsNullOrEmpty(phone))
            {
                TempData.SetNotification("Phone number is required.");
                return RedirectToAction("Index", "Home");
            }
            await apiService.PostAsync<bool>(
                "Log",
                new CreateMVCLogDTO { Page = MVCPages.RequestMoneyQr }
            );
            return View(new RequestMoneyQrVM { Phone = phone });
        }

        public async Task<IActionResult> RemoveFromCustomer(string phone, string customerId)
        {
            if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(customerId))
            {
                TempData.SetNotification("Phone and customerid are required.");
                return RedirectToAction("Index", "Home");
            }
            await apiService.PostAsync<bool>(
                "Log",
                new CreateMVCLogDTO { Page = MVCPages.RemoveAccountFromCustomer }
            );
            var response = await apiService.DeleteAsync<bool>(
                $"Account/Remove-From-Customer",
                new RemoveAccountFromCustomerDTO { Phone = phone, CustomerId = customerId }
            );
            if (response != null && response.IsSuccess)
            {
                TempData.SetNotification($"{response.Message}", "Success");
                if (HttpContext.User.IsInRole(Roles.Master))
                {
                    return RedirectToAction("AdminDashboard", "Home");
                }
                return RedirectToAction("Index", "Home");
            }
            TempData.SetNotification(
                response?.Message ?? "An error occurred while retrieving account information."
            );
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> DeleteAccount(string phone)
        {
            if (string.IsNullOrEmpty(phone))
            {
                TempData.SetNotification("Phone is required.");
                return RedirectToAction("Index", "Home");
            }
            await apiService.PostAsync<bool>(
                "Log",
                new CreateMVCLogDTO { Page = MVCPages.DeleteAccount }
            );
            var response = await apiService.DeleteAsync<bool>(
                $"Account/Remove",
                new DeleteAccountDTO { Phone = phone }
            );
            if (response != null && response.IsSuccess)
            {
                TempData.SetNotification($"{response.Message}", "Success");
                if (HttpContext.User.IsInRole(Roles.Master))
                {
                    return RedirectToAction("AdminDashboard", "Home");
                }
                return RedirectToAction("Index", "Home");
            }
            TempData.SetNotification(
                response?.Message ?? "An error occurred while retrieving account information."
            );
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> ActivateAccount(string phone)
        {
            if (string.IsNullOrEmpty(phone))
            {
                TempData.SetNotification("Phone is required.");
                return RedirectToAction("Index", "Home");
            }
            await apiService.PostAsync<bool>(
                "Log",
                new CreateMVCLogDTO { Page = MVCPages.ActivateAccount }
            );
            var response = await apiService.PostAsync<bool>(
                $"Account/Activate-Account",
                new DeleteAccountDTO { Phone = phone }
            );
            if (response != null && response.IsSuccess)
            {
                TempData.SetNotification($"{response.Message}", "Success");
                if (HttpContext.User.IsInRole(Roles.Master))
                {
                    return RedirectToAction("AdminDashboard", "Home");
                }
                return RedirectToAction("Index", "Home");
            }
            TempData.SetNotification(
                response?.Message ?? "An error occurred while retrieving account information."
            );
            return RedirectToAction("Index", "Home");
        }
    }
}
