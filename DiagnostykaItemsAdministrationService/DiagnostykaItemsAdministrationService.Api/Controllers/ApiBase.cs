using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DiagnostykaItemsAdministrationService.Api.Controllers;

/// <summary>
/// Base Api controller implementation
/// </summary>
[ApiController]
[Route("[controller]")]
public class ApiBase : ControllerBase
{
    /// <summary>
    /// Ecanpsulation for ISender.
    /// </summary>
    private ISender _sender;

    /// <summary>
    /// ISender from MediatR object.
    /// </summary>
    protected ISender Sender => _sender ??= HttpContext.RequestServices.GetService<ISender>() ?? throw new Exception("ISender not implemented");

    /// <summary>
    /// Gets created route.
    /// </summary>
    /// <typeparam name="T">type of Id. e.g int or guid.</typeparam>
    /// <param name="controller">name of controller.</param>
    /// <param name="id">id value.</param>
    /// <returns>Route path.</returns>
    protected string GetCreatedRoute<T>(string controller, T id)
    {
        return $"api/{controller.Replace("Controller", string.Empty)}/{id}".ToLower();
    }
}