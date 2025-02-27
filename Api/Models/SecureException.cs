namespace Api.Models;

public class SecureException : Exception
{
    public readonly string? QueryParameters;
    public readonly string? BodyParameters;
    public SecureException(string message, string? queryParameters = null, string? bodyParameters = null) : base(message)
    {
        this.QueryParameters = queryParameters;
        this.BodyParameters = bodyParameters;
    }
}