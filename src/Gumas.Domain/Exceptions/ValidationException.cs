namespace Gumas.Domain.Exceptions;

public class ValidationException : Exception
{
    public IDictionary<string, string[]> Errors { get; }

    public ValidationException() : base("Bir veya daha fazla doğrulama hatası oluştu.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(string message) : base(message)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(string message, Exception innerException) : base(message, innerException)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IDictionary<string, string[]> errors)
        : base("Bir veya daha fazla doğrulama hatası oluştu.")
    {
        Errors = errors;
    }

    public ValidationException(string propertyName, string errorMessage)
        : base("Bir veya daha fazla doğrulama hatası oluştu.")
    {
        Errors = new Dictionary<string, string[]>
        {
            { propertyName, new[] { errorMessage } }
        };
    }
}
