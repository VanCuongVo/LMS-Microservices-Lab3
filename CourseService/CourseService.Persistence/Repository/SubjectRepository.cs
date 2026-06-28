using CourseService.Domain.Entities;
using CourseService.Domain.IRepository;
using CourseService.Persistence.Context;

namespace CourseService.Persistence.Repository
{
    public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
    {
        public SubjectRepository(CourseDbContext context) : base(context)
        {
        }
    }
}
