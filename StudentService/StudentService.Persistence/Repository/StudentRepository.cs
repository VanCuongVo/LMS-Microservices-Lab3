using StudentService.Domain.Entities;
using StudentService.Domain.IRepository;
using StudentService.Persistence.Context;

namespace StudentService.Persistence.Repository
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(StudentDbContext context) : base(context)
        {
        }
    }
}
