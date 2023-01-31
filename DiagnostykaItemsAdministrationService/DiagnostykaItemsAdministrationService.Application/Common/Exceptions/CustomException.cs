using System.Runtime.Serialization;

namespace DiagnostykaItemsAdministrationService.Application.Common.Exceptions;

public abstract class CustomException : Exception
{
    public abstract int Status { get; }

    protected CustomException()
    {
    }

    protected CustomException(string message) : base(message)
    {
    }

    protected CustomException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected CustomException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public CustomException(string message, params object?[] args) : base(string.Format(message, args))
    {
    }
}
