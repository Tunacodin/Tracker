using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tracker.Api;
using Tracker.Constants;
using Tracker.Helpers;
using Tracker.Models;

namespace Tracker.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApiService apiService;

        public HomeController(ILogger<HomeController> logger, ApiService apiService)
        {
            _logger = logger;
            this.apiService = apiService;
        }

        public async Task<IActionResult> Index(string? email, int pageNumber = 1, int pageSize = 10)
        {
            ViewBag.Email = email;
            var response = await apiService.PostAsync<BalancesVMPaginated>(
                $"Customer/Dashboard?email={email}",
                new PaginationDTO { PageNumber = pageNumber, PageSize = pageSize }
            );
            if (response != null && response.IsSuccess)
            {
                await apiService.PostAsync<bool>(
                    "Log",
                    new CreateMVCLogDTO { Page = MVCPages.Index, TargetEmail = email }
                );
                return View(response.Data);
            }

            TempData.SetNotification(
                response?.Message ?? "An error occurred while retrieving customers."
            );
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = Roles.Master)]
        public async Task<IActionResult> AdminDashboard(int pageNumber = 1, int pageSize = 10)
        {
            var response = await apiService.PostAsync<PaginatedResponse<CustomerVM>>(
                "Customer/All-Customers",
                new PaginationDTO { PageNumber = pageNumber, PageSize = pageSize }
            );
            if (response != null && response.IsSuccess)
            {
                await apiService.PostAsync<bool>(
                    "Log",
                    new CreateMVCLogDTO { Page = MVCPages.AdminDashboard }
                );
                return View(response.Data);
            }
            TempData.SetNotification(
                response?.Message ?? "An error occurred while retrieving customers."
            );
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = Roles.Master)]
        public async Task<IActionResult> SearchCustomer(
            string search,
            int pageNumber = 1,
            int pageSize = 10
        )
        {
            if (string.IsNullOrEmpty(search))
            {
                TempData.SetNotification("Arama alanı boş olamaz");
                return RedirectToAction("AdminDashboard", "Home");
            }
            var response = await apiService.PostAsync<PaginatedResponse<CustomerVM>>(
                $"Customer/Search-Customer?search={search}",
                new PaginationDTO { PageNumber = pageNumber, PageSize = pageSize }
            );
            if (response != null && response.IsSuccess)
            {
                await apiService.PostAsync<bool>(
                    "Log",
                    new CreateMVCLogDTO { Page = MVCPages.AdminDashboard }
                );
                return View("AdminDashboard", response.Data);
            }
            TempData.SetNotification(
                response?.Message ?? "An error occurred while retrieving customers."
            );
            return RedirectToAction("AdminDashboard", "Home");
        }

        [Authorize(Roles = Roles.Master)]
        public async Task<IActionResult> CreateCustomer()
        {
            await apiService.PostAsync<bool>(
                "Log",
                new CreateMVCLogDTO { Page = MVCPages.CreateCustomer }
            );
            return View();
        }

        [HttpPost]
        [Authorize(Roles = Roles.Master)]
        public async Task<IActionResult> CreateCustomer(CreateCustomerDTO createCustomerDTO)
        {
            if (createCustomerDTO.PasswordConfirm != createCustomerDTO.Password)
            {
                ModelState.AddModelError("", "Passwords should match");
                return View(createCustomerDTO);
            }
            var response = await apiService.PostAsync<bool>("Customer", createCustomerDTO);

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction("AdminDashboard", "Home");
            }
            ModelState.AddModelError("", $"Create failed : {response?.Message}");
            return View(createCustomerDTO);
        }

        public async Task<IActionResult> GenerateCode()
        {
            var accountsResponse = await apiService.GetAsync<List<AccountVM>>(
                "Customer/Customer-Accounts"
            );
            if (User.IsInRole(Roles.Master))
            {
                var response = await apiService.GetAsync<List<CustomerVM>>("Customer");
                if (
                    response != null
                    && response.IsSuccess
                    && accountsResponse != null
                    && accountsResponse.IsSuccess
                )
                {
                    await apiService.PostAsync<bool>(
                        "Log",
                        new CreateMVCLogDTO { Page = MVCPages.GenerateCode }
                    );
                    return View(
                        new GenerateCodeVM()
                        {
                            Customers = response.Data,
                            Accounts = accountsResponse.Data,
                        }
                    );
                }

                TempData.SetNotification(
                    response?.Message ?? "An error occurred while retrieving customer emails."
                );
                return RedirectToAction("Index", "Home");
            }
            await apiService.PostAsync<bool>(
                "Log",
                new CreateMVCLogDTO { Page = MVCPages.GenerateCode }
            );
            return View(new GenerateCodeVM { Accounts = accountsResponse.Data });
        }

        public async Task<IActionResult> VerifyPanelCode(string? email)
        {
            await apiService.PostAsync<bool>(
                "Log",
                new CreateMVCLogDTO { Page = MVCPages.VerifyPanel, TargetEmail = email }
            );
            return View(new VerifyCodeDTO { CustomerEmail = email ?? "" });
        }

        [HttpPost]
        public async Task<IActionResult> VerifyPanelCode(VerifyCodeDTO model)
        {
            var response = await apiService.PostAsync<bool>($"Customer/Verify-Code", model);
            if (response != null && response.IsSuccess)
            {
                TempData.SetNotification(response?.Message, "Success");
                if (HttpContext.User.IsInRole(Roles.Master))
                {
                    return RedirectToAction("AdminDashboard", "Home");
                }
                return RedirectToAction("Index", "Home");
            }

            TempData.SetNotification(
                response?.Message ?? "An error occurred while adding account to the customer"
            );
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = Roles.Master)]
        public async Task<IActionResult> AdminLogPage([FromQuery] LogFilterDTO filterVM)
        {
            // Initialize default values if needed
            filterVM ??= new LogFilterDTO();
            if (filterVM.PageNumber <= 0)
            {
                filterVM.PageNumber = 1;
            }
            if (filterVM.PageSize <= 0)
            {
                filterVM.PageSize = 10;
            }

            var response = await apiService.GetAsync<PaginatedResponse<LogVM>>(
                $"Log?{BuildQueryString(filterVM)}"
            );

            if (response != null && response.IsSuccess)
            {
                ViewBag.Filter = filterVM; // Pass filter back to view
                return View(response.Data);
            }

            TempData.SetNotification(
                response?.Message ?? "An error occurred while retrieving logs."
            );
            return View(new PaginatedResponse<LogVM>());
        }

        private string BuildQueryString(LogFilterDTO filter)
        {
            var queryParams = new List<string>();

            if (!string.IsNullOrWhiteSpace(filter.TargetAccount))
                queryParams.Add($"TargetAccount={Uri.EscapeDataString(filter.TargetAccount)}");

            if (!string.IsNullOrWhiteSpace(filter.TargetEmail))
                queryParams.Add($"TargetEmail={Uri.EscapeDataString(filter.TargetEmail)}");

            if (!string.IsNullOrWhiteSpace(filter.LoggedInUserEmail))
                queryParams.Add(
                    $"LoggedInUserEmail={Uri.EscapeDataString(filter.LoggedInUserEmail)}"
                );

            if (!string.IsNullOrWhiteSpace(filter.Page))
                queryParams.Add($"Page={Uri.EscapeDataString(filter.Page)}");

            if (filter.ActionType.HasValue)
                queryParams.Add($"ActionType={filter.ActionType}");

            if (filter.ProcessStatus.HasValue)
                queryParams.Add($"ProcessStatus={filter.ProcessStatus}");

            queryParams.Add($"PageNumber={filter.PageNumber}");
            queryParams.Add($"PageSize={filter.PageSize}");

            return string.Join("&", queryParams);
        }
    }
}
