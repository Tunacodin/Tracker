using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.LogDTOs;
using Application.Services;
using Application.Utilities.Constants;
using Application.Utilities.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TrackerAPI.Controllers
{
    public class LogController : BaseController
    {
        public LogController(ILoggerService loggerService)
            : base(loggerService) { }

        [HttpPost]
        public async Task<IActionResult> CreateMvcLog(CreateMVCLogDTO dto)
        {
            string message;
            if (string.IsNullOrEmpty(HttpContext.Items["Email"] as string))
            {
                message = $"Login sayfasina girildi";
            }
            else
            {
                message = $"{HttpContext.Items["Email"]} kullanıcısı {dto.Page} sayfasına girdi";
            }
            await _loggerService.LogForMVCAsync(
                message,
                HttpContext.Items["Email"] as string,
                dto.Page,
                dto.TargetEmail
            );
            return Ok(new GenericResponse<bool> { IsSuccess = true });
        }

        [HttpGet]
        [Authorize(Roles = Roles.Master)]
        public async Task<IActionResult> GetFilteredLogs([FromQuery] LogFilterDTO filterDto)
        {
            var result = await _loggerService.GetFilteredLogsAsync(filterDto);
            return Ok(result);
        }
    }
}
