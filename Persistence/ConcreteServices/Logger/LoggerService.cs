using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.LogDTOs;
using Application.Repositories.ILoggerReadRepositories;
using Application.Services;
using Application.Utilities.Response;
using Application.VMs;
using AutoMapper;
using Domain.Enums;
using Domain.HelperClass;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Persistence.ConcreteServices.Logger
{
    // Infrastructure/Services/Logger/TrackerLoggerService.cs
    public class LoggerService : ILoggerService
    {
        private readonly ILoggerWriteRepository _writeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerReadRepository _readRepository;
        private readonly IMapper mapper;

        public LoggerService(
            ILoggerWriteRepository writeRepository,
            IHttpContextAccessor httpContextAccessor,
            ILoggerReadRepository readRepository,
            IMapper mapper
        )
        {
            _writeRepository = writeRepository;
            _httpContextAccessor = httpContextAccessor;
            _readRepository = readRepository;
            this.mapper = mapper;
        }

        public async Task LogForApiAsync(
            string message,
            string? email = null,
            ActionType? action = null,
            ProcessStatus? processStatus = ProcessStatus.Started,
            string? targetEmail = null,
            string? targetAccount = null
        )
        {
            var log = new TrackerLog
            {
                Message = $"{message} - {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}",
                LoggedInUserEmail = email ?? GetCurrentUserEmail(),
                ActionType = action,
                IpAddress = GetClientIpAddress(),
                TargetEmail = targetEmail,
                ProcessStatus = processStatus,
                TargetAccount = targetAccount,
            };

            await _writeRepository.AddAsync(log);
            await _writeRepository.SaveAsync();
        }

        public async Task LogForMVCAsync(
            string message,
            string? loggedInUserEmail = null,
            string? page = null,
            string? targetEmail = null
        )
        {
            var log = new TrackerLog
            {
                Message = $"{message} - {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}",
                LoggedInUserEmail = loggedInUserEmail ?? GetCurrentUserEmail(),
                Page = page,
                TargetEmail = targetEmail,
                IpAddress = GetClientIpAddress(),
            };

            await _writeRepository.AddAsync(log);
            await _writeRepository.SaveAsync();
        }

        public async Task<GenericResponse<PaginatedResponse<LogVM>>> GetFilteredLogsAsync(
            LogFilterDTO filterDto
        )
        {
            var query = _readRepository.Table.AsQueryable().AsNoTracking();
            query = ApplyFilters(query, filterDto);
            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)filterDto.PageSize);

            var logs = await query
                .OrderByDescending(x => x.CreationDate)
                .Skip((filterDto.PageNumber - 1) * filterDto.PageSize)
                .Take(filterDto.PageSize)
                .ToListAsync();

            return new GenericResponse<PaginatedResponse<LogVM>>
            {
                Data = new PaginatedResponse<LogVM>
                {
                    Data = mapper.Map<List<LogVM>>(logs),
                    PageNumber = filterDto.PageNumber,
                    PageSize = filterDto.PageSize,
                    TotalPages = totalPages,
                    TotalCount = totalCount,
                },
                IsSuccess = true,
            };
        }

        private IQueryable<TrackerLog> ApplyFilters(
            IQueryable<TrackerLog> query,
            LogFilterDTO filterDto
        )
        {
            if (!string.IsNullOrWhiteSpace(filterDto.TargetAccount))
                query = query.Where(x => x.TargetAccount == filterDto.TargetAccount);

            if (!string.IsNullOrWhiteSpace(filterDto.TargetEmail))
                query = query.Where(x => x.TargetEmail == filterDto.TargetEmail);

            if (!string.IsNullOrWhiteSpace(filterDto.LoggedInUserEmail))
                query = query.Where(x => x.LoggedInUserEmail == filterDto.LoggedInUserEmail);

            if (!string.IsNullOrWhiteSpace(filterDto.Page))
                query = query.Where(x => x.Page == filterDto.Page);

            if (filterDto.ActionType.HasValue)
                query = query.Where(x => x.ActionType == filterDto.ActionType);

            if (filterDto.ProcessStatus.HasValue)
                query = query.Where(x => x.ProcessStatus == filterDto.ProcessStatus);

            return query;
        }

        private string? GetCurrentUserEmail()
        {
            return _httpContextAccessor.HttpContext?.Items["Email"] as string;
        }

        private string? GetClientIpAddress()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                return _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
            }
            return null;
        }

        public void Write(string msg)
        {
            throw new NotImplementedException();
        }
    }
}
