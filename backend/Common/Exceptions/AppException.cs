using System.Net;

namespace PhoneStoreAPI.Common.Exceptions;

public class AppException : Exception
{
    public HttpStatusCode StatusCode { get; }

    public AppException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest) 
        : base(message)
    {
        StatusCode = statusCode;
    }
}

public class NotFoundException : AppException
{
    public NotFoundException(string message) 
        : base(message, HttpStatusCode.NotFound)
    {
    }
}

public class ConflictException : AppException
{
    public ConflictException(string message) 
        : base(message, HttpStatusCode.Conflict)
    {
    }
}
