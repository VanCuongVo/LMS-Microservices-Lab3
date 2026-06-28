using System.Linq;
using Microsoft.EntityFrameworkCore;
using CourseService.Application.DTOs.Request;
using CourseService.Domain.Entities;

namespace CourseService.Application.Utility
{
    public static class CourseQueryExtension
    {
        public static IQueryable<Course> Search(this IQueryable<Course> query, QueryParameters request)
        {
            if (string.IsNullOrWhiteSpace(request.Search))
            {
                return query;
            }
            var search = request.Search.ToLower();
            return query.Where(x => x.Coursename.ToLower().Contains(search));
        }

        public static IQueryable<Course> Sort(this IQueryable<Course> query, QueryParameters request)
        {
            return request.Sort switch
            {
                "courseName" => query.OrderBy(x => x.Coursename),
                "-courseName" => query.OrderByDescending(x => x.Coursename),
                _ => query.OrderBy(x => x.Courseid)
            };
        }

        public static IQueryable<Course> Paging(this IQueryable<Course> query, QueryParameters request)
        {
            return query.Skip((request.Page - 1) * request.Size).Take(request.Size);
        }

        public static IQueryable<Course> Expand(this IQueryable<Course> query, QueryParameters request)
        {
            if (string.IsNullOrWhiteSpace(request.Expand))
            {
                return query;
            }
            var expands = request.Expand.Split(",");

            foreach (var item in expands)
            {
                var val = item.Trim().ToLower();
                if (val == "enrollment" || val == "student")
                {
                    query = query.Include(x => x.Enrollments);
                }
            }
            return query;
        }

        public static object SelectFields<T>(this IEnumerable<T> source, string? fields)
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                return source;
            }

            var result = new List<Dictionary<string, object>>();
            var fieldList = fields.Split(",");

            foreach (var item in source)
            {
                var data = new Dictionary<string, object>();
                foreach (var field in fieldList)
                {
                    var property = typeof(T).GetProperty(
                        field.Trim(),
                        System.Reflection.BindingFlags.IgnoreCase |
                        System.Reflection.BindingFlags.Public |
                        System.Reflection.BindingFlags.Instance);

                    if (property != null)
                    {
                        data[property.Name] = property.GetValue(item) ?? "";
                    }
                }
                result.Add(data);
            }
            return result;
        }
    }
}
