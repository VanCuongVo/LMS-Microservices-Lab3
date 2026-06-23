using IdentityService.Domain.Entities;

namespace IdentityService.Domain.IRepository
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        Task<RefreshToken?> GetByTokenAsync(string token);

    }
}