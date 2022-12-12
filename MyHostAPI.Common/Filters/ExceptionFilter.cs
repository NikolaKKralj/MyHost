using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using MyHostAPI.Common.Enums;

namespace MyHostAPI.Common.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private static readonly IReadOnlyDictionary<string, int> _errorCodes = new Dictionary<string, int>
        {
            { ErrorSource.Request.ToString(), 400 },
            { ErrorSource.Authentication.ToString(), 401 },
            { ErrorSource.Authorization.ToString(), 403 },
            { ErrorSource.Database.ToString(), 404 },
            { ErrorSource.Conflict.ToString(), 409 },
            { ErrorSource.Application.ToString(), 500 },
        };

        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var errorResponse = GetErrorResponse(context);

            var result = new JsonResult(errorResponse)
            {
                StatusCode = errorResponse.Code,
                ContentType = "application/json"
            };

            context.Result = result;

            _logger.LogError("Error Message: " + errorResponse.Message + ", StatusCode: " + errorResponse.Code);
        }

        private static ErrorResponse GetErrorResponse(ExceptionContext exceptionContext)
        {
            var exception = exceptionContext.Exception;

            var errorResponse = new ErrorResponse
            {
                Message = exception.Message,
                StackTrace = exception.StackTrace,
                Path = exceptionContext.HttpContext.Request.Path,
            };

            var isErrorCodeFound = _errorCodes.TryGetValue(exception.Source, out var errorCode);

            if (isErrorCodeFound)
            {
                errorResponse.Type = exception.Source;
                errorResponse.Code = errorCode;
            }

            return errorResponse;
        }
    }
}
