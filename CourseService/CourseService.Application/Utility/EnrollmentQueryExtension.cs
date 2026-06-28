using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CourseService.Application.DTOs.Request;
using CourseService.Domain.Entities;

namespace CourseService.Application.Utility
{
    public static class EnrollmentQueryExtension
    {
        public static IQueryable<Enrollment> Search(
            this IQueryable<Enrollment> query,
            QueryParameters request,
            List<int>? matchingStudentIds = null)
        {
            if (string.IsNullOrEmpty(request.Search))
            {
                return query;
            }

            var search = request.Search.ToLower();

            if (matchingStudentIds != null && matchingStudentIds.Any())
            {
                return query.Where(x =>
                    x.Course.Coursename.ToLower().Contains(search) ||
                    x.Status.ToString().ToLower().Contains(search) ||
                    matchingStudentIds.Contains(x.Studentid));
            }

            return query.Where(x =>
                x.Course.Coursename.ToLower().Contains(search) ||
                x.Status.ToString().ToLower().Contains(search));
        }

        public static IQueryable<Enrollment> Sort(
            this IQueryable<Enrollment> query,
            QueryParameters request)
        {
            return request.Sort switch
            {
                "courseName" => query.OrderBy(x => x.Course.Coursename),
                "-courseName" => query.OrderByDescending(x => x.Course.Coursename),
                "enrollDate" => query.OrderBy(x => x.Enrolldate),
                "-enrollDate" => query.OrderByDescending(x => x.Enrolldate),
                "status" => query.OrderBy(x => x.Status),
                "-status" => query.OrderByDescending(x => x.Status),
                _ => query.OrderBy(x => x.Enrollmentid)
            };
        }

        public static IQueryable<Enrollment> Paging(
            this IQueryable<Enrollment> query,
            QueryParameters request)
        {
            return query.Skip((request.Page - 1) * request.Size).Take(request.Size);
        }

        public static IQueryable<Enrollment> Expand(this IQueryable<Enrollment> query, QueryParameters request)
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
                    query = query.Include(x => x.Course);
                }
            }
            return query;
        }
    }
}
