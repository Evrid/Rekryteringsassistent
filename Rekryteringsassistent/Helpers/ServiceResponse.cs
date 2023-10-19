namespace Rekryteringsassistent.Helpers;


public class ServiceResponse<T>
{
    public T? Data { get; set; }
    public bool Success { get; set; } = true;
    public string? Message { get; set; }
    public int ErrorCode { get; set; }

}

public class ServiceResponse
{
    public bool Success { get; set; } = true;
    public string? Message { get; set; }
    public int ErrorCode { get; set; }
}
