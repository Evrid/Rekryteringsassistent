using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rekryteringsassistent.DTO;
using Rekryteringsassistent.Helpers;
using Rekryteringsassistent.Services;

namespace Rekryteringsassistent.Controllers;

/*
 * POST /api/users/register: Registrera en ny användare.
   POST /api/users/login: Autentisera användare och generera en sessionstoken.
   GET /api/users/me: Hämta den för närvarande inloggade användarens profil.
   PUT /api/users/me: Uppdatera den för närvarande inloggade användarens profil.
 */
[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    readonly UserService _userService;
    readonly TokenService _tokenService;

    public UserController(UserService userService, TokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }

 

    [HttpPost("register")]
    public async Task<ActionResult<LoggedInUserDto>> Register([FromBody] RegisterUserDto model)
    {
        var registrationResult = await _userService.RegisterUserAsync(model);

        if (registrationResult.Success)
        {
            return StatusCode(201, registrationResult.Data);
        }

        return StatusCode(registrationResult.ErrorCode,
            new ResponseMessage { Success = false, Message = registrationResult.Message });
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginDto model)
    {
        var loginResult = await _userService.Authenticate(model);

        if (loginResult.Success)
        {
            return StatusCode(201, loginResult.Data);
        }

        return StatusCode(loginResult.ErrorCode,
            new ResponseMessage { Success = false, Message = loginResult.Message });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult> GetCurrentUserProfile()
    {
        var userId = GetUserIdFromToken();

        var result = await _userService.GetUser(userId);

        if (result.Success)
        {
            return StatusCode(200, result.Data);
        }

        return StatusCode(result.ErrorCode,
            new ResponseMessage { Success = false, Message = result.Message });

    }

    [HttpPut("me")]
    public async Task<ActionResult> UpdateCurrentUserProfile([FromBody] UpdateUserDto model)
    {
        var userId = GetUserIdFromToken();
        var result =  await _userService.UpdateUserAsync(userId, model);
        return result.Success 
            ? StatusCode(200, result.Data) 
            : StatusCode(result.ErrorCode, new ResponseMessage { Success = false, Message = result.Message });
    }

    private int GetUserIdFromToken()
    {
        return _tokenService.GetUserId(Request.Headers["Authorization"]!);
    }
}