using System;
using System.Threading.Tasks;
using CourseService.Domain.IRepository;

namespace CourseService.Domain.IUnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ICourseRepository Courses { get; }
        ISemesterRepository Semesters { get; }
        ISubjectRepository Subjects { get; }
        IEnrollmentRepository Enrollments { get; }
        Task<int> SaveChangesAsync();
    }
}
