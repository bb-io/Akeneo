namespace Apps.Akeneo.Models.Response;

public class ErrorResponse
{
    public string Message { get; set; }
    public IEnumerable<ErrorResponse>? Errors { get; set; }
}