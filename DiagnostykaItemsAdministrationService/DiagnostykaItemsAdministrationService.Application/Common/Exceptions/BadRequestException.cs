using System.Net;

namespace DiagnostykaItemsAdministrationService.Application.Common.Exceptions;

public class BadRequestException : CustomException
{
	public override int Status => (int)HttpStatusCode.NotFound;

	public BadRequestException()
			: base()
	{
	}

	public BadRequestException(string message)
			: base(message)
	{
	}

	public BadRequestException(string message, Exception innerException)
			: base(message, innerException)
	{
	}
}
