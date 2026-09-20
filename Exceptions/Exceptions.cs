namespace metalurgicaMVC.Exceptions;

public class AppException(string mensaje, Exception exception) : Exception(mensaje)
{
    public Exception ExcepcionOriginal { get; set; } = exception;
}

public class ClientException(string mensaje) : Exception(mensaje)
{
}