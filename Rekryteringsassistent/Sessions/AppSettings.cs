namespace Rekryteringsassistent.Sessions;

public class AppSettings
{
    public string? ValidAudience { get; set; }
    public string? ValidIssuer { get; set; }
    public string? Secret { get; set; }
    public int JWtTokenValidMinutes { get; set; }
    public int JWtTimeLeftReminderMinutes { get; set; }
    public string? JwtIssuer { get; set; }
    public string? JwtAudience { get; set; }
    public bool RequireHttpsMetadata { get; set; }
}
