using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace DiagnostykaItemsAdministrationService.Application.Common.Exceptions.Filters;
public class HttpResponseExceptionFilter : IExceptionFilter
{
    private readonly IHostEnvironment _hostEnvironment;

    public HttpResponseExceptionFilter(IHostEnvironment hostEnvironment) =>
            _hostEnvironment = hostEnvironment;

    public void OnException(ExceptionContext context)
    {
        if (context.Exception is not CustomException exception) return;

        var errorResponse = _hostEnvironment.IsDevelopment() ?
                new BaseExceptionModel(exception.Status, exception.Message, exception.StackTrace) :
                new BaseExceptionModel(exception.Status, exception.Message);

        context.Result = new JsonResult(exception)
        {
            StatusCode = exception.Status < 600 ? exception.Status : 500,
            Value = errorResponse,
        };
        context.ExceptionHandled = true;
    }
}
