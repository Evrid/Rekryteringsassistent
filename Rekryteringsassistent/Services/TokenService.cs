using Microsoft.IdentityModel.Tokens;
using Rekryteringsassistent.Helpers;
using Rekryteringsassistent.Models;
using Rekryteringsassistent.Sessions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Rekryteringsassistent.Services;

public class TokenService
{
    readonly IConfiguration _config;
    readonly ILogger<TokenService> _logger;

    public TokenService(IConfiguration config, ILogger<TokenService> logger)
    {
        _config = config;
        _logger = logger;
    }

    

    public ServiceResponse<string> GetTokenFromHeaders(IHeaderDictionary headers)
    {
        var token = headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token is null)
        {
            return new ServiceResponse<string>
            {
                Success = false,
                ErrorCode = 500,
                Message = "Failed to retrieve user token from headers"
            };
        }

        return new ServiceResponse<string> { Data = token };
    }

    public bool ValidateToken(string authToken)
    {
        var appSettings = GetAppSettings();

        authToken = CleanToken(authToken);

        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters =
            GetValidationParameters(appSettings.JwtAudience!, appSettings.JwtIssuer!, appSettings.Secret!);

        tokenHandler.ValidateToken(authToken, validationParameters, out _);
        return true;
    }

    
    public int GetUserId(string authorizaiton)
    {
        var stringId = GetClaimByKey(authorizaiton, "id");
        return Convert.ToInt32(stringId);
    }

    public string GetClaimByKey(string? authToken, string key)
    {
        var data = string.Empty;

        if (authToken != null && authToken.StartsWith("Bearer"))
        {
            var stream = CleanToken(authToken);

            var handler = new JwtSecurityTokenHandler();

            if (handler.ReadToken(stream) is not JwtSecurityToken token) throw new Exception("Failed to read the token.");

            var collection = token.Claims;
            foreach (var item in collection)
            {
                if (item.Type == key)
                {
                    data = item.Value;
                }

            }

        }
        else
        {
            throw new Exception("The authorization header is either empty or isn't Basic.");
        }

        return data;
    }

    public string CreateJwtToken(ApplicationUser user, int timeValidity = 0)
    {
        var appSettings = GetAppSettings();
        var claims = GenerateClaims(user, appSettings);

        var tokenTimeValidity = Math.Max(appSettings.JWtTokenValidMinutes, timeValidity);

        var jwt = new JwtSecurityToken(
            issuer: appSettings.JwtIssuer,
            audience: appSettings.JwtAudience,
            claims: claims,
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddMinutes(tokenTimeValidity),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.Secret!)),
                SecurityAlgorithms.HmacSha256)
        );

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        return encodedJwt;
    }

    

    private static TokenValidationParameters GetValidationParameters(string audience, string issuer, string secretKey)
    {
        return new TokenValidationParameters()
        {
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)) // The same key as the one that generate the token
        };

    }

    private AppSettings GetAppSettings()
    {



        var audience = _config.GetValue<string>("JWT:JwtAudience");
        var issuer = _config.GetValue<string>("JWT:JwtIssuer");
        var secretKey = _config.GetValue<string>("JWT:Secret");
        var tokenValidMinutes = _config.GetValue<int>("JWT:JWtTokenValidMinutes");
        var timeLeftReminderMinutes = _config.GetValue<int>("JWT:JWtTimeLeftReminderMinutes");



        if (audience == null || issuer == null || secretKey == null)
            throw new InvalidOperationException("Missing critical Jwt settings in 'appsettings.json'.");


        return new AppSettings
        {
            JWtTokenValidMinutes = tokenValidMinutes,
            JWtTimeLeftReminderMinutes = timeLeftReminderMinutes,
            JwtAudience = audience,
            JwtIssuer = issuer,
            Secret = secretKey
        };


    }

    private static string CleanToken(string authToken)
    {
        var stream = authToken;
        if (authToken.Contains("Bearer ")) stream = authToken["Bearer ".Length..].Trim();
        return stream;
    }

    private static IEnumerable<Claim> GenerateClaims(ApplicationUser user, AppSettings appSettings)
    {
        var now = DateTime.Now;

        return new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString(), ClaimValueTypes.Integer32),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(now).ToString(), ClaimValueTypes.Integer64),
            new(ClaimTypes.Name, user.Id.ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new("issuer", appSettings.JwtIssuer!),
            new("audience", appSettings.JwtAudience!),
            new("id", user.Id.ToString()),
            // Add act on behalf of self claim.
            new(ClaimTypes.Role, "privatperson"),
            new("http://schemas.danica.se/identity/claims/actonbehalfof", user.Id.ToString())
        };
    }

    private static long ToUnixEpochDate(DateTime date)
    {
        return (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}