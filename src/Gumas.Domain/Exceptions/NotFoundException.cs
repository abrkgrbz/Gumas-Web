namespace Gumas.Domain.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException() : base("İstenen kaynak bulunamadı.")
    {
    }

    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public NotFoundException(string entityName, object key)
        : base($"{entityName} bulunamadı. Anahtar: {key}")
    {
    }
}
