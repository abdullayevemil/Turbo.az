namespace Turboaz.Core.CustomExceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string? message) : base(message) { }
}