using CoreProject.Models;
using System.IdentityModel.Tokens.Jwt;

namespace CoreProject.Services;

public class ActiveUser
{

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<ActiveUser> _logger;
    User user { get; }

    public ActiveUser(IHttpContextAccessor httpContextAccessor, ILogger<ActiveUser> logger)
    {
        _logger.LogInformation("start ActiveUser Constructor");
        this.user = new User();
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        // GetTokenFromCookies();
        _logger.LogInformation("end ActiveUser Constructor");
    }

    // public void GetTokenFromCookies()
    // {
    //     // קבלת הטוקן מהקוקיז
    //     if (_httpContextAccessor.HttpContext.Request.Cookies.TryGetValue("AuthToken", out var token))
    //     {
    //         if (token == null)
    //             return;
    //         var handler = new JwtSecurityTokenHandler();

    //         // פענוח הטוקן
    //         var jwtToken = handler.ReadJwtToken(token);

    //         // הוצאת הערכים (claims)
    //         var claims = jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);
    //         this.user.Id = int.Parse(claims["id"]);
    //         this.user.Name = claims["name"];
    //         this.user.type = claims["type"];
    //         // Console.WriteLine("this.user.Id");
    //         // Console.WriteLine(user.Name);
    //         // return user; // מחזיר את הטוקן
    //     }
    //     // return null; // אם לא נמצא טוקן
    // }

    // public User GetActiveUser() => user;

    public User GetActiveUser(string token)
    {
        _logger.LogInformation("start ActiveUser GetActiveUser");
        var handler = new JwtSecurityTokenHandler();

        // פענוח הטוקן
        var jwtToken = handler.ReadJwtToken(token);

        // הוצאת הערכים (claims)
        var claims = jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);
        this.user.Id = int.Parse(claims["id"]);
        this.user.Name = claims["name"];
        this.user.type = claims["type"];
        if (user == null)
        {
            _logger.LogInformation("in ActiveUser GetActiveUser - User is not authorized.");
            throw new UnauthorizedAccessException("User is not authorized.");
        }
        _logger.LogInformation("end ActiveUser GetActiveUser");
        return user;
    }
}