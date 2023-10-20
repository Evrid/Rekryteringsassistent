namespace Rekryteringsassistent.DTO;

public class UpdateUserDto
{
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Gender { get; set; }
    public string? MobilePhoneNumber { get; set; }
    public bool? AgreeMarketing { get; set; }
    public bool? SubscribeToEmailNotification { get; set; }
    public object? ProfileImage { get; set; }
}


