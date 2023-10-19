using Rekryteringsassistent.Helpers;
using Rekryteringsassistent.Models;

namespace Rekryteringsassistent.Interfaces;

public interface TokenService
{
    bool ValidateToken(string authToken);
    int GetUserId(string authorizaiton);
    string GetClaimByKey(string authToken, string key);
    string CreateJwtToken(ApplicationUser user, int timeValidity = 0);
    ServiceResponse<string> GetTokenFromHeaders(IHeaderDictionary headers);
    


}

