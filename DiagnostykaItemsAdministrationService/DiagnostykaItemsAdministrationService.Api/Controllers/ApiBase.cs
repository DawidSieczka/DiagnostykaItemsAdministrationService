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
}