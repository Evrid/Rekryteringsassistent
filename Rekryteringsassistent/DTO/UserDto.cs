namespace Rekryteringsassistent.DTO;

public class UserDto
{
    public int? Id { get; set; }
    public string? UserName { get; set; }
    public string? SocialSecurityNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? FullName { get; set; }
    public string? Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? MobilePhoneNumber { get; set; }
    public bool AgreeMarketing { get; set; }
    public bool SubscribeToEmailNotification { get; set; }
    public string? ProfileImage { get; set; }
}

