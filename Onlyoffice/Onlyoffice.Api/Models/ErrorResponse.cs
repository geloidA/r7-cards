namespace Onlyoffice.Api.Models;

public class ErrorResponse
{
    public int Status { get; set; }
    public int StatusCode { get; set; }
    public Error? Error { get; set; }
}

public class Error
{
    public string? Message { get; set; }
    public int Hresult { get; set; }
    public object? Data { get; set; }
}
