using System;
using System.Collections.Generic;
using System.Linq;
using StudentService.Application.DTOs.Request;
using StudentService.Domain.Entities;

namespace StudentService.Application.Utility
{
    public static class StudentQueryExtensions
    {
        public static IQueryable<Student> Search(this IQueryable<Student> query, QueryParameters request)
        {
            if (string.IsNullOrEmpty(request.Search))
            {
                return query;
            }
            var search = request.Search.ToLower();
            return query.Where(x => x.Fullname.ToLower().Contains(search));
        }

        public static IQueryable<Student> Sort(this IQueryable<Student> query, QueryParameters request)
        {
            return request.Sort switch
            {
                "fullName" => query.OrderBy(x => x.Fullname),
                "-fullName" => query.OrderByDescending(x => x.Fullname),
                "dateOfBirth" => query.OrderBy(x => x.Dateofbirth),
                "-dateOfBirth" => query.OrderByDescending(x => x.Dateofbirth),
                _ => query.OrderBy(x => x.Studentid)
            };
        }

        public static IQueryable<Student> Paging(this IQueryable<Student> query, QueryParameters request)
        {
            return query.Skip((request.Page - 1) * request.Size).Take(request.Size);
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
