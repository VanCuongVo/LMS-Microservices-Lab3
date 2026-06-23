using IdentityService.Domain.IRepository;
using IdentityService.Domain.IUnitOfWork;
using IdentityService.Persistence.Context;

namespace IdentityService.Persistence.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppIdentityDbContext _context;

        public IRefreshTokenRepository RefreshTokens { get; }

        public ILogRepository LogRepository { get; }

        public UnitOfWork(
            AppIdentityDbContext context,
            IRefreshTokenRepository refreshTokenRepository,
            ILogRepository logRepository)
        {
            _context = context;

            RefreshTokens = refreshTokenRepository;
            LogRepository = logRepository;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        {

            return await _context.SaveChangesAsync();
        }
    }
}