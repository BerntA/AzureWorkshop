using Microsoft.AspNetCore.Mvc;

namespace Portal.Extensions;

public static class Auth
{
    public static Guid? GetUserId(this Controller obj)
    {
        string userIdString = obj.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;
        return Guid.Parse(userIdString);
    }
}
