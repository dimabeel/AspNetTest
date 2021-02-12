using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace AutoLotAPICore.Filters
{
    public class AutoLotExceptionFilter : IExceptionFilter
    {
        public AutoLotExceptionFilter(IWebHostEnvironment webHostEnvironment)
        {
            isDevelopment = webHostEnvironment.IsDevelopment();
        }

        public void OnException(ExceptionContext exceptionContext)
        {
            var ex = exceptionContext.Exception;
            string stackTrace = (isDevelopment) ? ex.StackTrace : string.Empty;
            IActionResult actionResult;
            string message = ex.Message;

            if (ex is DbUpdateConcurrencyException)
            {
                if (!isDevelopment)
                {
                    message = $"There was an error updating the database. " +
                        $"Another user has altered the record.";
                }

                var additionalData =
                    new
                    {
                        Error = "Concurrency Issue.",
                        Message = message,
                        StackTrace = stackTrace
                    };
                actionResult = new BadRequestObjectResult(additionalData);
            }
            else
            {
                if (!isDevelopment)
                {
                    message = "There was an unknown error. Please try again.";
                }

                var additionalData =
                    new
                    {
                        Error = "General error.",
                        Message = message,
                        StackTrace = stackTrace,
                    };
                actionResult = new ObjectResult(additionalData)
                {
                    StatusCode = 500
                };
            }

            exceptionContext.Result = actionResult;
        }

        private bool isDevelopment;
    }
}
