using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Rekryteringsassistent.DTO;
using Rekryteringsassistent.Extensions;
using Rekryteringsassistent.Helpers;
using Rekryteringsassistent.Models;

namespace Rekryteringsassistent.Services;

public class UserService
{
    readonly IMapper _mapper;
    readonly UserManager<ApplicationUser> _userManager;
    readonly ILogger<UserService> _logger;
    readonly TokenService _tokenService;

    public UserService(IMapper mapper, UserManager<ApplicationUser> userManager,
        ILogger<UserService> logger, TokenService tokenService)
    {
        _mapper = mapper;
        _userManager = userManager;
        _logger = logger;
        _tokenService = tokenService;
    }

    public async Task<ServiceResponse<LoggedInUserDto>> RegisterUserAsync(RegisterUserDto model)
    {

        if (model.UserName.IsNullOrEmpty())  model.UserName = model.Email;

        ApplicationUser user = _mapper.Map<ApplicationUser>(model);


        IdentityResult? result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            var errorMessages = new List<string>();

            foreach (var error in result.Errors)
            {
                _logger.LogError($"Code: {error.Code}, Description: {error.Description}");
                errorMessages.Add(error.Description);
            }

            string combinedErrorMessage = string.Join(" ", errorMessages);

            return new ServiceResponse<LoggedInUserDto>
            {
                Success = false, Message = $"Failed to create the new user. Errors: {combinedErrorMessage}"
            };
        }

        ApplicationUser? savedApplicationUser = await _userManager.FindByEmailAsync(model.Email);
        if (savedApplicationUser != null)
        {
            // todo: error handling in token generator
            try
            {
                var token = _tokenService.CreateJwtToken(savedApplicationUser);
                var userDto = _mapper.Map<UserDto>(savedApplicationUser);
                var loggedInApplicationUser = new LoggedInUserDto { User = userDto, Token = token };

                return new ServiceResponse<LoggedInUserDto> { Data = loggedInApplicationUser };

            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while generating token.");

                return new ServiceResponse<LoggedInUserDto>
                {
                    Success = false,
                    Message = "An unexpected error occurred while generating a token.",
                    ErrorCode = 500
                };
            }
        }

        return new ServiceResponse<LoggedInUserDto> { Success = false, Message = "Failed to create a new user" };
    }

    
    public async Task<ServiceResponse<LoggedInUserDto>> Authenticate(LoginDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.EmailOrUsername!) == null
            ? await _userManager.FindByNameAsync(model.EmailOrUsername!)
            : await _userManager.FindByEmailAsync(model.EmailOrUsername!);

        if (user == null)
        {
            return new ServiceResponse<LoggedInUserDto>
            {
                Success = false, Message = "User not found.", ErrorCode = 404
            };
        }

        if (!await _userManager.CheckPasswordAsync(user, model.Password!))
        {
            return new ServiceResponse<LoggedInUserDto>
            {
                Success = false, Message = "Wrong password.", ErrorCode = 401
            };
        }


        try
        {
            var token = _tokenService.CreateJwtToken(user);
            var loggedInUser = new LoggedInUserDto { User = _mapper.Map<UserDto>(user), Token = token };
            
            return new ServiceResponse<LoggedInUserDto> { Data = loggedInUser };

        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while generating token.");

            return new ServiceResponse<LoggedInUserDto>
            {
                Success = false, Message = "An unexpected error occurred while generating a token.", ErrorCode = 500
            };
        }
        
    }

    public async Task<ServiceResponse<UserDto>> GetUser(int userId)
    {

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return new ServiceResponse<UserDto>
            {
                Success = false, ErrorCode = 404, Message = $"User {userId} was not found."
            };
        }

        return new ServiceResponse<UserDto> { Data = _mapper.Map<UserDto>(user)};
    }
}
