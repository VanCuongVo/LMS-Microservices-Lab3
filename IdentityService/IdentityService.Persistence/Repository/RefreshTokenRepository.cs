using IdentityService.Domain.Entities;
using IdentityService.Domain.IRepository;
using IdentityService.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Persistence.Repository
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {

        protected readonly AppIdentityDbContext _context;


        public RefreshTokenRepository(AppIdentityDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _context.RefreshTokens.Include(x => x.User).FirstOrDefaultAsync(x => x.Token == token);
        }
    }
}