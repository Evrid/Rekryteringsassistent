using System.ComponentModel.DataAnnotations;

namespace Rekryteringsassistent.DTO;

public class LoginDto
{
    [Required(ErrorMessage = "Username or email is required")]
    public string? EmailOrUsername { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
}
