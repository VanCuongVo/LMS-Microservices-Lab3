using IdentityService.Domain.Entities;

namespace IdentityService.Application.IServices
{
    public interface IJwtService
    {
        Task<string> GenerateTokenAsync(ApplicationUser user);
        public string GenerateRefreshToken();
    }
}
