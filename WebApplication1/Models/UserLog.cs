namespace WebApplication1.Models
{
    public class UserLog
    {
        public string UserName { get; set; } =string.Empty;
        public string Password { get; set; } =string.Empty;
        public string? Token;

    }
    public class AuthenticatedResponse
    {
        public string? Tokenc { get; set; }
    }
}
