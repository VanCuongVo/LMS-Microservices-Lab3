using IdentityService.Domain.Entities;

namespace IdentityService.Application.Interfaces
{
    public interface ILogService
    {
        public Task WriteAsync(Log log);
    }
}