namespace IdentityService.Application.DTOs.Response
{
    public class LoginResponse
    {
        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int ExpiresIn { get; set; }
    }
}
