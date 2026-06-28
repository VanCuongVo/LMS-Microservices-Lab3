using System;
using System.Threading.Tasks;
using StudentService.Domain.IRepository;
using StudentService.Domain.IUnitOfWork;
using StudentService.Persistence.Context;

namespace StudentService.Persistence.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StudentDbContext _context;

        public IStudentRepository Students { get; }

        public UnitOfWork(StudentDbContext context, IStudentRepository studentRepository)
        {
            _context = context;
            Students = studentRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
