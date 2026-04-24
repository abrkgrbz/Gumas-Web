namespace Gumas.Domain.Exceptions;

public class BusinessException : Exception
{
    public string Code { get; }

    public BusinessException() : base("İş kuralı ihlali.")
    {
        Code = "BUSINESS_ERROR";
    }

    public BusinessException(string message) : base(message)
    {
        Code = "BUSINESS_ERROR";
    }

    public BusinessException(string message, string code) : base(message)
    {
        Code = code;
    }

    public BusinessException(string message, Exception innerException) : base(message, innerException)
    {
        Code = "BUSINESS_ERROR";
    }

    public BusinessException(string message, string code, Exception innerException) : base(message, innerException)
    {
        Code = code;
    }
}
