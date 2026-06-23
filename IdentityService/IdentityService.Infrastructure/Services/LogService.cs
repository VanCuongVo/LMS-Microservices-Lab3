using IdentityService.Application.Interfaces;
using IdentityService.Domain.Entities;
using IdentityService.Domain.IUnitOfWork;

namespace IdentityService.Infrastructure.Services
{
    public class LogService : ILogService
    {
        private readonly IUnitOfWork _uow;

        public LogService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task WriteAsync(Log log)
        {
            await _uow.LogRepository.AddAsync(log);
            await _uow.SaveChangesAsync();
        }
    }
}