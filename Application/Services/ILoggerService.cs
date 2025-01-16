using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.LogDTOs;
using Application.Utilities.Response;
using Application.VMs;
using Domain.Enums;
using Domain.HelperClass;

namespace Application.Services
{
    public interface ILoggerService
    {
        public void Write(string msg);
        Task LogForApiAsync(
            string message,
            string? email = null,
            ActionType? action = null,
            ProcessStatus? processStatus = ProcessStatus.Started,
            string? targetEmail = null,
            string? targetAccount = null
        );
        Task LogForMVCAsync(
            string message,
            string? loggedInUserEmail = null,
            string? page = null,
            string? targetEmail = null
        );
        Task<GenericResponse<PaginatedResponse<LogVM>>> GetFilteredLogsAsync(
            LogFilterDTO filterDto
        );
    }
}
