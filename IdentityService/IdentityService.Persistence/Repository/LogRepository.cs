using IdentityService.Domain.Entities;
using IdentityService.Domain.IRepository;
using IdentityService.Persistence.Context;

namespace IdentityService.Persistence.Repository
{
    public class LogRepository : GenericRepository<Log>, ILogRepository
    {
        public LogRepository(AppIdentityDbContext context) : base(context)
        {
        }
    }
}