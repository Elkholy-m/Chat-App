namespace ChatApp.Domain.Entities;

public class Error
{
    public int StatusCode { get; set; }

    public string Hint { get; set; } = null!;

    public string Message { get; set; } = null!;
}
