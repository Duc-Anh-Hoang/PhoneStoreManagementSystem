using System.Net;

namespace PhoneStoreAPI.Common.Exceptions;

public class AppException : Exception
{
    public HttpStatusCode StatusCode { get; }
    public List<string>? Errors { get; }

    public AppException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest, List<string>? errors = null) 
        : base(message)
    {
        StatusCode = statusCode;
        Errors = errors;
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
    public ConflictException(string message, List<string>? errors = null) 
        : base(message, HttpStatusCode.Conflict, errors)
    {
    }
}

public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message = "Tài khoản hoặc mật khẩu không chính xác.") 
        : base(message, HttpStatusCode.Unauthorized)
    {
    }
}

public class ForbiddenException : AppException
{
    public ForbiddenException(string message = "Bạn không có quyền thực hiện chức năng này.") 
        : base(message, HttpStatusCode.Forbidden)
    {
    }
}

public class BadRequestException : AppException
{
    public BadRequestException(string message, List<string>? errors = null) 
        : base(message, HttpStatusCode.BadRequest, errors)
    {
    }
}
