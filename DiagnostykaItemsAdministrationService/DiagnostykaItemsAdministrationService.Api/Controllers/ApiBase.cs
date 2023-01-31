using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DiagnostykaItemsAdministrationService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ApiBase : ControllerBase
{

	private ISender _sender;

	protected ISender Sender => _sender ??= HttpContext.RequestServices.GetService<ISender>() ?? throw new Exception("ISender not implemented");

}