namespace Rekryteringsassistent.DTO;

public class LoggedInUserDto
{
    public UserDto? User { get; set; }
    public string? Token { get; set; }
}
