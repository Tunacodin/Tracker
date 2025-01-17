using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.DTOs;
using Application.DTOs.AccountDTOs;
using Application.DTOs.AuthDTOs;
using Application.DTOs.CodeDTOs;
using Application.DTOs.CustomerDTOs;
using Application.Repositories.IAccountRepositories;
using Application.Repositories.ICodeRepositories;
using Application.Repositories.ICustomerRepositories;
using Application.Services;
using Application.Utilities.Constants;
using Application.Utilities.Helper;
using Application.Utilities.Response;
using Application.VMs;
using AutoMapper;
using Azure;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using static QRCoder.PayloadGenerator;

namespace Persistence.ConcreteServices.CustomerService
{
    public class CustomerService : ICustomerService
    {
        private readonly UserManager<Customer> userManager;
        private readonly IMapper mapper;
        private readonly ITokenGeneratorService tokenGenerator;
        private readonly IApiService apiService;
        private readonly IAccountReadRepository accountReadRepository;
        private readonly IAccountWriteRepository accountWriteRepository;
        private readonly IAccountService accountService;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ICustomerReadRepository customerReadRepository;
        private readonly ICodeWriteRepository codeWriteRepository;
        private readonly ICodeReadRepository codeReadRepository;
        private readonly ICustomerWriteRepository customerWriteRepository;
        private readonly IHttpContextAccessor httpContext;
        private readonly IEmailService emailService;

        public CustomerService(
            UserManager<Customer> userManager,
            IMapper mapper,
            ITokenGeneratorService tokenGenerator,
            IApiService apiService,
            IAccountReadRepository accountReadRepository,
            IAccountWriteRepository accountWriteRepository,
            IAccountService accountService,
            RoleManager<IdentityRole> roleManager,
            ICustomerReadRepository customerReadRepository,
            ICodeWriteRepository codeWriteRepository,
            ICodeReadRepository codeReadRepository,
            ICustomerWriteRepository customerWriteRepository,
            IHttpContextAccessor httpContext,
            IEmailService emailService


        )
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.tokenGenerator = tokenGenerator;
            this.apiService = apiService;
            this.accountReadRepository = accountReadRepository;
            this.accountWriteRepository = accountWriteRepository;
            this.accountService = accountService;
            this.roleManager = roleManager;
            this.customerReadRepository = customerReadRepository;
            this.codeWriteRepository = codeWriteRepository;
            this.codeReadRepository = codeReadRepository;
            this.customerWriteRepository = customerWriteRepository;
            this.httpContext = httpContext;
            this.emailService = emailService;
        }

        public async Task<GenericResponse<Token>> CreateCustomerAsync(CreateCustomerDTO model)
        {
            Customer customer = mapper.Map<Customer>(model);
            IdentityResult result = await userManager.CreateAsync(customer, model.Password);
            await userManager.AddToRoleAsync(
                customer,
                model.IsMaster ? Roles.Master : Roles.Customer
            );

            // GenericResponse<Token> kullanıyoruz
            GenericResponse<Token> response = new();

            if (result.Succeeded)
            {
                // Access Token oluştur
                Token token = tokenGenerator.CreateAccesToken(
                    60 * 60 * 24, // Token süresi (1 gün)
                    customer,
                    new List<string> { model.IsMaster ? Roles.Master : Roles.Customer }
                );

                // Başarılı yanıt
                response.Data = token;
                response.IsSuccess = true;
                response.Message = Messages.CustomerCreated; // "Müşteri başarıyla oluşturuldu."
            }
            else
            {
                // Hata durumunda yanıt
                response.IsSuccess = false;
                foreach (var error in result.Errors)
                    response.Message += $"{error.Code} - {error.Description}\n";
            }

            return response;
        }

        public async Task<GenericResponse<bool>> LoginCustomerAsync(LoginDTO model)
        {
            var response = new GenericResponse<bool>();

            // Kullanıcıyı email ile bul
            Customer customer = await customerReadRepository
                .GetWhere(c => c.Email == model.Email)
                .FirstOrDefaultAsync();

            if (customer == null)
            {
                response.IsSuccess = false;
                response.Message = Messages.CustomerNotFound;
                return response;
            }

            // Şifre kontrolü
            bool isLogin = await userManager.CheckPasswordAsync(customer, model.Password);
            if (!isLogin)
            {
                response.IsSuccess = false;
                response.Message = Messages.LoginFail;
                return response;
            }

            try
            {
                // Doğrulama kodu gönder
                string verificationCode = GenerateVerificationCode();
                customer.VerificationCode = verificationCode;
                customer.VerificationCodeExpiration = DateTime.UtcNow.AddMinutes(5);
                await userManager.UpdateAsync(customer);
                await emailService.SendEmailAsync(customer.Email, "Doğrulama Kodu", $"Doğrulama kodunuz: {verificationCode}");

                response.IsSuccess = true;
                response.Message = Messages.VerificationCodeSent;
                response.Data = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Data = false;
            }
            return response;
        }

        private static string GenerateVerificationCode()
        {
            return new Random().Next(100000, 999999).ToString(); // 6 haneli rastgele bir kod
        }

        public async Task<GenericResponse<Token>> VerifyLoginCodeAsync(string email, string verificationCode)
        {
            var response = new GenericResponse<Token>();

            Customer? customer = await customerReadRepository
                .GetWhere(c => c.Email == email)
                .FirstOrDefaultAsync();

            if (customer is null)
            {
                response.IsSuccess = false;
                response.Message = Messages.CustomerNotFound;
                return response;
            }

            if (customer.VerificationCode != verificationCode || customer.VerificationCodeExpiration < DateTime.UtcNow)
            {
                response.IsSuccess = false;
                response.Message = Messages.InvalidOrExpiredCode;
                return response;
            }

            // Token oluştur
            var userRoles = await userManager.GetRolesAsync(customer);
            Token token = tokenGenerator.CreateAccesToken(60 * 60 * 24, customer, userRoles);
            token.Id = customer.Id;

            // Doğrulama kodunu temizle
            customer.VerificationCode = null;
            customer.VerificationCodeExpiration = null;
            await userManager.UpdateAsync(customer);

            response.IsSuccess = true;
            response.Message = Messages.LoginSuccess;
            response.Data = token;
            return response;
        }
        public async Task<GenericResponse<BalancesVM>> GetBalancesAsync(string email)
        {
            Customer customer = await customerReadRepository.GetCustomerByEmail(email);
            GenericResponse<BalancesVM> response = new();
            if (customer == null)
            {
                response.IsSuccess = false;
                response.Message = Messages.CustomerNotFound;
                return response;
            }
            response.Data = new BalancesVM() { CustomerId = customer.Id };
            foreach (Account account in customer.Accounts)
            {
                var accountResponse = await accountService.GetBalance(
                    new GetBalanceDTO { AccountId = account.Id }
                );
                response.Data.Accounts.Add(accountResponse.Data ?? new AccountVM());
            }
            return response;
        }

        public async Task<GenericResponse<BalancesVMPaginated>> GetBalancesAsyncPaginated(
            string email,
            PaginationDTO pagination
        )
        {
            var response = new GenericResponse<BalancesVMPaginated>();

            var customer = await userManager.FindByEmailAsync(email);
            if (customer == null)
            {
                response.IsSuccess = false;
                response.Message = Messages.CustomerNotFound;
                return response;
            }
            bool isAdmin = await userManager.IsInRoleAsync(customer, "Master");
            var paginatedAccounts =
                await accountReadRepository.GetAccountsByCustomerIdAsyncPaginated(
                    customer.Id,
                    pagination,
                    isAdmin
                );
            response.Data = new BalancesVMPaginated()
            {
                Data = new List<AccountVM>(),
                CustomerId = customer.Id,
                PageNumber = paginatedAccounts.PageNumber,
                PageSize = paginatedAccounts.PageSize,
                TotalPages = paginatedAccounts.TotalPages,
                TotalCount = paginatedAccounts.TotalCount,
            };
            foreach (Account account in paginatedAccounts.Data)
            {
                var accountResponse = await accountService.GetBalance(
                    new GetBalanceDTO { AccountId = account.Id }
                );
                response.Data.Data.Add(accountResponse.Data ?? new AccountVM());
            }
            response.IsSuccess = true;
            return response;
        }

        public async Task<GenericResponse<List<AccountVM>>> GetCustomerAccounts(string email)
        {
            GenericResponse<List<AccountVM>> response = new() { Data = new List<AccountVM>() };
            Customer? customer = await customerReadRepository
                .GetWhere(c => c.Email == email, true)
                .FirstOrDefaultAsync();
            if (customer == null)
            {
                response.IsSuccess = false;
                response.Message = Messages.CustomerNotFound;
                return response;
            }
            var isAdmin = await userManager.IsInRoleAsync(customer, Roles.Master);
            if (isAdmin)
            {
                customer.Accounts = accountReadRepository.GetAll(true).ToList();
            }
            else
            {
                customer.Accounts = await accountReadRepository.GetAccountsByCustomerIdAsync(
                    customer.Id,
                    true
                );
            }
            response.Data = customer.Accounts.Select(a => mapper.Map<AccountVM>(a)).ToList();
            return response;
        }

        public async Task<GenericResponse<List<CustomerVM>>> GetAllCustomers()
        {
            GenericResponse<List<CustomerVM>> response = new();
            try
            {
                var customers = userManager.Users.ToList();
                response.Data = customers.Select(c => mapper.Map<CustomerVM>(c)).ToList();
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
        }

        public async Task<GenericResponse<PaginatedResponse<CustomerVM>>> GetAllCustomersPaginated(
            PaginationDTO pagination
        )
        {
            var response = new GenericResponse<PaginatedResponse<CustomerVM>>();
            try
            {
                var query = userManager.Users.AsQueryable();

                // Calculate pagination values
                int totalCount = await query.CountAsync();
                var customers = await query
                    .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .ToListAsync();

                response.Data = new PaginatedResponse<CustomerVM>
                {
                    Data = customers.Select(c => mapper.Map<CustomerVM>(c)).ToList(),
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)pagination.PageSize),
                };

                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
        }

        public async Task<GenericResponse<CodeVM>> TransferAccountToCustomer(
            string email,
            PhoneNumbers accounts
        )
        {
            GenericResponse<CodeVM> response = new();
            if (accounts.Accounts.Count <= 0)
            {
                response.IsSuccess = false;
                response.Message = Messages.NotExist;
                return response;
            }
            var customer = await customerReadRepository.GetCustomerByEmail(email);
            if (customer == null)
            {
                response.IsSuccess = false;
                response.Message = Messages.CustomerNotFound;
                return response;
            }
            if (
                accounts.Accounts.Any(incomingPhone =>
                    !customer.Accounts.Any(customerAccount =>
                        customerAccount.Phone == incomingPhone
                    )
                )
            )
            {
                response.IsSuccess = false;
                response.Message = Messages.SelectedCustomerDoesNotHaveAccounts;
                return response;
            }
            Code code = new() { CustomerId = customer.Id, AccountPhoneNumbers = accounts.Accounts };
            await codeWriteRepository.AddAsync(code);
            bool result = await codeWriteRepository.SaveAsync() > 0;
            if (!result)
            {
                response.IsSuccess = false;
                response.Message = Messages.SaveFail;
                return response;
            }
            response.Data = mapper.Map<CodeVM>(code);
            response.Message = Messages.AddSucceeded;
            return response;
        }

        public async Task<GenericResponse<bool>> VerifyCode(VerifyCodeDTO model)
        {
            GenericResponse<bool> response = new();
            var code = await codeReadRepository.GetByIdAsync(model.CodeId);
            if (code == null)
            {
                response.IsSuccess = false;
                response.Message = Messages.NotExist;
                return response;
            }
            var customer = await customerReadRepository.GetCustomerByEmail(model.CustomerEmail);
            if (customer == null)
            {
                response.IsSuccess = false;
                response.Message = Messages.CustomerNotFound;
                return response;
            }
            var accounts = accountReadRepository
                .GetWhere(a => code.AccountPhoneNumbers.Contains(a.Phone))
                .ToList();
            var existingAccountIds = customer.Accounts.Select(a => a.Id).ToList();
            var newAccounts = accounts.Where(a => !existingAccountIds.Contains(a.Id)).ToList();
            customer.Accounts.AddRange(newAccounts);
            var result = await userManager.UpdateAsync(customer);
            if (!result.Succeeded)
            {
                response.IsSuccess = false;
                response.Message = Messages.SaveFail;
                return response;
            }
            response.Data = true;
            response.Message = Messages.AddSucceeded;
            codeWriteRepository.Remove(code);
            await codeWriteRepository.SaveAsync();
            return response;
        }

        public async Task<GenericResponse<bool>> DeleteCustomer(string email)
        {
            GenericResponse<bool> response = new();
            var customer = await userManager.FindByEmailAsync(email);
            if (customer == null)
            {
                response.IsSuccess = false;
                response.Message = Messages.CustomerNotFound;
                return response;
            }
            customerWriteRepository.Remove(customer);
            await customerWriteRepository.SaveAsync();
            response.Data = true;
            response.Message = Messages.DeleteSucceeded;
            return response;
        }

        public async Task<GenericResponse<bool>> MakeActiveCustomer(string email)
        {
            GenericResponse<bool> response = new();
            var customer = await userManager.FindByEmailAsync(email);
            if (customer == null)
            {
                response.IsSuccess = false;
                response.Message = Messages.CustomerNotFound;
                return response;
            }
            customer.Status = Status.Modified;
            await userManager.UpdateAsync(customer);
            response.Data = true;
            response.Message = Messages.UpdateSucceeded;
            return response;
        }

        public async Task<GenericResponse<PaginatedResponse<CustomerVM>>> SearchCustomer(
            string search,
            PaginationDTO pagination
        )
        {
            GenericResponse<PaginatedResponse<CustomerVM>> response = new();
            var customer = await userManager.FindByEmailAsync(
                httpContext.HttpContext.Items["Email"] as string
            );
            if (customer == null)
            {
                response.IsSuccess = false;
                response.Message = Messages.CustomerNotFound;
                return response;
            }
            var customers = await customerReadRepository.SearchCustomerPaginated(
                search,
                pagination
            );

            var paginatedResponse = new PaginatedResponse<CustomerVM>
            {
                Data = mapper.Map<List<CustomerVM>>(customers.Data),
                PageNumber = customers.PageNumber,
                PageSize = customers.PageSize,
                TotalPages = customers.TotalPages,
                TotalCount = customers.TotalCount,
            };

            response.Data = paginatedResponse;
            response.IsSuccess = true;
            return response;
        }

        public Task<GenericResponse<Token>> GenerateToken(string email)
        {
            var response = new GenericResponse<Token>();

            try
            {
                // Token süresi
                var tokenExpiration = DateTime.UtcNow.AddHours(1);

                // JWT içeriği
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Benzersiz Token ID
        };

                // Token güvenlik anahtarı ve şifreleme algoritması
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("trackerapp icin hazirlanacak secretkey"));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // Token oluşturma
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = tokenExpiration,
                    SigningCredentials = credentials,
                    Issuer = "www.trackerapp.com",
                    Audience = "www.trackerapp.com"
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(securityToken);

                // Token yanıtını oluştur
                response.IsSuccess = true;
                response.Data = new Token
                {
                    AccessToken = tokenString,
                    Expiration = tokenExpiration
                };
                response.Message = "Token başarıyla oluşturuldu.";
            }
            catch (Exception ex)
            {
                // Hata durumunda mesaj ve loglama
                response.IsSuccess = false;
                response.Message = $"Token oluşturulamadı: {ex.Message}";
                Console.WriteLine($"GenerateToken Hatası: {ex.Message}");
            }

            return Task.FromResult(response);
        }

    }
}
