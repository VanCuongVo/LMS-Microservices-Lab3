using CourseService.Domain.Entities;
using CourseService.Domain.IRepository;
using CourseService.Persistence.Context;

namespace CourseService.Persistence.Repository
{
    public class SemesterRepository : GenericRepository<Semester>, ISemesterRepository
    {
        public SemesterRepository(CourseDbContext context) : base(context)
        {
        }
    }
}
