using Microsoft.AspNetCore.Identity;

namespace Rekryteringsassistent.Models;

public class ApplicationUser : IdentityUser<int>
{
    // Properties inherited from IdentityUser:
    // string Id ------------------------------------------- NOW int
    // string UserName
    // string NormalizedUserName
    // string Email
    // string NormalizedEmail
    // bool EmailConfirmed
    // string PasswordHash
    // string SecurityStamp
    // string ConcurrencyStamp
    // string PhoneNumber
    // bool PhoneNumberConfirmed
    // bool TwoFactorEnabled
    // DateTimeOffset? LockoutEnd
    // bool LockoutEnabled
    // int AccessFailedCount

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? FullName { get; set; }
    public string? Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? SocialSecurityNumber { get; set; }
    public string? MobilePhoneNumber { get; set; }
    public bool AgreeMarketing { get; set; }
    public bool SubscribeToEmailNotification { get; set; }
    public string? ProfileImage { get; set; }
    public string? Application { get; set; }

}
