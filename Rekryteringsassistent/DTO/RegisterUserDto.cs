using System.ComponentModel.DataAnnotations;
#pragma warning disable CS8618

namespace Rekryteringsassistent.DTO;

public class RegisterUserDto
{
    
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
    

    public string? UserName { get; set; }
    public string? FirstName { get; set; }  
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? SocialSecurityNumber { get; set; }

    public string? MobilePhoneNumber { get; set; }
    public string? PhoneNumber { get; set; }

    public bool AgreeMarketing { get; set; } = false;
    public bool SubscribeToEmailNotification { get; set; } = false;


}
