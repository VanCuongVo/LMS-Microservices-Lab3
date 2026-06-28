using System.Linq;
using CourseService.Application.DTOs.Request;
using CourseService.Domain.Entities;

namespace CourseService.Application.Utility
{
    public static class SubjectQueryExtensions
    {
        public static IQueryable<Subject> Search(this IQueryable<Subject> query, QueryParameters request)
        {
            if (string.IsNullOrEmpty(request.Search))
            {
                return query;
            }
            return query.Where(x => x.Subjectname.ToLower().Contains(request.Search.ToLower()));
        }

        public static IQueryable<Subject> Sort(this IQueryable<Subject> query, QueryParameters request)
        {
            return request.Sort switch
            {
                "subjectName" => query.OrderBy(x => x.Subjectname),
                "-subjectName" => query.OrderByDescending(x => x.Subjectname),
                _ => query.OrderBy(x => x.Subjectid)
            };
        }

        public static IQueryable<Subject> Paging(this IQueryable<Subject> query, QueryParameters request)
        {
            return query.Skip((request.Page - 1) * request.Size).Take(request.Size);
        }
    }
}
