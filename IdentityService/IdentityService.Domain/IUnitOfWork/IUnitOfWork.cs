using IdentityService.Domain.IRepository;

namespace IdentityService.Domain.IUnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRefreshTokenRepository RefreshTokens { get; }
        ILogRepository LogRepository { get; }

        Task<int> SaveChangesAsync();
    }
}