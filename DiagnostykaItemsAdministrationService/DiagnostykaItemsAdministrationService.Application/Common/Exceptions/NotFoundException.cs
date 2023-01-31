using System.Net;

namespace DiagnostykaItemsAdministrationService.Application.Common.Exceptions;

public class NotFoundException : CustomException
{
	public override int Status => (int)HttpStatusCode.NotFound;

	public NotFoundException()
			: base()
	{
	}

	public NotFoundException(string message)
			: base(message)
	{
	}

	public NotFoundException(string message, Exception innerException)
			: base(message, innerException)
	{
	}
}
