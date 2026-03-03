namespace ChatApp.Domain.Configurations;

public class JwtConfiguration
{
    public int EXPIRATION { get; set; }
    public string ISSER { get; set; } = null!;
    public string AUDIENCE { get; set; } =null!;
}
