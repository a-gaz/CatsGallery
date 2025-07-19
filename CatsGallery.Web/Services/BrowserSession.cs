namespace CatsGallery.Web.Services;

public class BrowserSession : IBrowserSession
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BrowserSession(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetCurrentUserId()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        var userId = session?.GetString("UserId");
        
        if (userId == null)
        {
            userId = Guid.NewGuid().ToString();
            session?.SetString("UserId", userId);
        }
        
        return userId;
    }
}