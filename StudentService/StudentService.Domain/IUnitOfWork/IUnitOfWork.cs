using System;
using System.Threading.Tasks;
using StudentService.Domain.IRepository;

namespace StudentService.Domain.IUnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IStudentRepository Students { get; }
        Task<int> SaveChangesAsync();
    }
}
