using CourseService.Domain.Entities;
using CourseService.Domain.IRepository;
using CourseService.Persistence.Context;

namespace CourseService.Persistence.Repository
{
    public class EnrollmentRepository : GenericRepository<Enrollment>, IEnrollmentRepository
    {
        public EnrollmentRepository(CourseDbContext context) : base(context)
        {
        }
    }
}
