using CourseService.Domain.Entities;
using CourseService.Domain.IRepository;
using CourseService.Persistence.Context;

namespace CourseService.Persistence.Repository
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(CourseDbContext context) : base(context)
        {
        }
    }
}
