using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services;
using Application.Utilities.Response;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace TrackerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        protected readonly ILoggerService _loggerService;

        protected BaseController(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }

        protected async Task LogApiActionAsync(
            string message,
            ActionType actionType,
            ProcessStatus status,
            string? targetEmail = null,
            string? targetAccount = null
        )
        {
            string currentUserEmail = HttpContext.Items["Email"] as string;

            await _loggerService.LogForApiAsync(
                message,
                currentUserEmail,
                actionType,
                status,
                targetEmail,
                targetAccount
            );
        }

        protected async Task LogResultAsync<T>(
            GenericResponse<T> response,
            ActionType actionType,
            string actionDescription,
            string? targetEmail = null,
            string? targetAccount = null
        )
        {
            string currentUserEmail = HttpContext.Items["Email"] as string;
            string logMessage = string.IsNullOrEmpty(targetEmail)
                ? $"{actionDescription} hesap: {currentUserEmail}"
                : $"{actionDescription} hesap: {currentUserEmail} - hedef hesap: {targetEmail}";
            if (response.IsSuccess)
            {
                await LogApiActionAsync(
                    $"{logMessage} basarili oldu",
                    actionType,
                    ProcessStatus.Completed,
                    targetEmail,
                    targetAccount
                );
            }
            else
            {
                await LogApiActionAsync(
                    $"{logMessage} basarisiz oldu - {response.Message}",
                    actionType,
                    ProcessStatus.Failed,
                    targetEmail,
                    targetAccount
                );
            }
        }
    }
}
