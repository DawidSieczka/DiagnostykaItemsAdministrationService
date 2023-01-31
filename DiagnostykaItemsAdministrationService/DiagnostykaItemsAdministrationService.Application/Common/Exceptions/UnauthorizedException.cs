using System.Net;

namespace DiagnostykaItemsAdministrationService.Application.Common.Exceptions;

public class UnauthorizedException : CustomException
{
	public override int Status => (int)HttpStatusCode.Unauthorized;

	public UnauthorizedException()
			: base()
	{
	}

	public UnauthorizedException(string message)
			: base(message)
	{
	}

	public UnauthorizedException(string message, Exception innerException)
			: base(message, innerException)
	{
	}

	public UnauthorizedException(string id, object key)
			: base($"Id \"{id}\" for ({key}) was unathorized.")
	{
	}
}
