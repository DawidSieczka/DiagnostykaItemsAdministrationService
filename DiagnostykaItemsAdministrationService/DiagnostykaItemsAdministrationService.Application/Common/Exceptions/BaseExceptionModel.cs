using System.Text.Json.Serialization;

namespace DiagnostykaItemsAdministrationService.Application.Common.Exceptions;

public class BaseExceptionModel
{
	public int StatusCode { get; set; }
	public string Message { get; set; }

	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string? StackTrace { get; set; }

	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string? InnerExceptionMessage { get; set; }

	public BaseExceptionModel(int statusCode, string message, string? stackTrace = null, string? innerExceptionMessage = null)
	{
		StatusCode = statusCode;
		Message = message;
		StackTrace = stackTrace;
		InnerExceptionMessage = innerExceptionMessage;
	}
}
