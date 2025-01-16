using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Utilities.Constants;
using Application.Utilities.Response;
using Microsoft.AspNetCore.Http;

namespace Persistence.Middlewares
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                var errorResponse = new GenericResponse<object>
                {
                    IsSuccess = false,
                    Message = Messages.Fail,
                    Data = ex.Message,
                };

                response.StatusCode = StatusCodes.Status500InternalServerError;

                await response.WriteAsync(JsonSerializer.Serialize(errorResponse));
            }
        }
    }
}
