using IdentityService.Application.DTOs.Request;
using IdentityService.Application.DTOs.Response;

namespace IdentityService.Application.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterRequest request);

        Task<LoginResponse?> LoginAsync(LoginRequest request);
        Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request);
    }
}
