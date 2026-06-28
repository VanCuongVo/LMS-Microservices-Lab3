using System.Linq;
using Microsoft.EntityFrameworkCore;
using CourseService.Application.DTOs.Request;
using CourseService.Domain.Entities;

namespace CourseService.Application.Utility
{
    public static class SemesterQueryExtensions
    {
        public static IQueryable<Semester> Search(this IQueryable<Semester> query, QueryParameters request)
        {
            if (string.IsNullOrWhiteSpace(request.Search))
            {
                return query;
            }

            var search = request.Search.ToLower();
            return query.Where(x => x.Semestername.ToLower().Contains(search));
        }

        public static IQueryable<Semester> Sort(this IQueryable<Semester> query, QueryParameters request)
        {
            return request.Sort switch
            {
                "semestername" => query.OrderBy(x => x.Semestername),
                "-semestername" => query.OrderByDescending(x => x.Semestername),
                _ => query.OrderBy(x => x.Semesterid)
            };
        }

        public static IQueryable<Semester> Paging(this IQueryable<Semester> query, QueryParameters request)
        {
            return query.Skip((request.Page - 1) * request.Size).Take(request.Size);
        }

        public static IQueryable<Semester> Expand(this IQueryable<Semester> query, QueryParameters request)
        {
            if (string.IsNullOrWhiteSpace(request.Expand))
            {
                return query;
            }

            var expands = request.Expand.Split(",");
            foreach (var item in expands)
            {
                if (item.Trim().ToLower() == "course")
                {
                    query = query.Include(x => x.Courses);
                }
            }
            return query;
        }
    }
}
