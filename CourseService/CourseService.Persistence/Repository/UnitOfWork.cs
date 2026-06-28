using System;
using System.Threading.Tasks;
using CourseService.Domain.IRepository;
using CourseService.Domain.IUnitOfWork;
using CourseService.Persistence.Context;

namespace CourseService.Persistence.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CourseDbContext _context;

        public ICourseRepository Courses { get; }
        public ISemesterRepository Semesters { get; }
        public ISubjectRepository Subjects { get; }
        public IEnrollmentRepository Enrollments { get; }

        public UnitOfWork(
            CourseDbContext context,
            ICourseRepository courseRepository,
            ISemesterRepository semesterRepository,
            ISubjectRepository subjectRepository,
            IEnrollmentRepository enrollmentRepository)
        {
            _context = context;
            Courses = courseRepository;
            Semesters = semesterRepository;
            Subjects = subjectRepository;
            Enrollments = enrollmentRepository;
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
