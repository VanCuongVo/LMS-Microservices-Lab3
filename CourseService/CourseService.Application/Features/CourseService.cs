using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CourseService.Application.DTOs.Request;
using CourseService.Application.DTOs.Response;
using CourseService.Application.Extensions;
using CourseService.Application.Interfaces;
using CourseService.Application.Utility;
using CourseService.Domain.Entities;
using CourseService.Domain.IUnitOfWork;

namespace CourseService.Application.Features
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<CourseResponse>> CreateAsync(CreateCourseRequest request)
        {
            var course = new Course
            {
                Coursename = request.CourseName,
                Semesterid = request.SemesterId,
                Coursecode = request.Coursecode
            };

            var res = await _unitOfWork.Courses.AddAsync(course);
            await _unitOfWork.SaveChangesAsync();

            var courseFromDb = await _unitOfWork.Courses.GetQueryable()
                .Include(x => x.Semester)
                .FirstOrDefaultAsync(x => x.Courseid == res.Courseid);

            return new ApiResponse<CourseResponse>
            {
                success = true,
                message = "Create course successfully",
                Data = courseFromDb!.ToCourseResponse()
            };
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(id);
            if (course != null)
            {
                await _unitOfWork.Courses.DeleteAsync(course.Courseid);
                await _unitOfWork.SaveChangesAsync();
                return new ApiResponse<bool>
                {
                    success = true,
                    message = "Delete course successfully",
                    Data = true
                };
            }

            return new ApiResponse<bool>
            {
                success = false,
                message = "Delete course fails",
                Data = false
            };
        }

        public async Task<ApiResponse<object>> GetAllAsync(QueryParameters query)
        {
            var coursesQuery = _unitOfWork.Courses.GetQueryable()
                .Include(x => x.Semester)
                .Search(query)
                .Expand(query);

            var totalItems = await coursesQuery.CountAsync();
            var courses = await coursesQuery.Sort(query).Paging(query).ToListAsync();
            var response = courses.ToCourseResponseList();

            // Populate Student details if expand contains student
            if (!string.IsNullOrEmpty(query.Expand) && query.Expand.Split(',').Any(x => x.Trim().Equals("student", StringComparison.OrdinalIgnoreCase)))
            {
                var studentIds = courses
                    .SelectMany(c => c.Enrollments)
                    .Select(e => e.Studentid)
                    .Distinct()
                    .ToList();

                // Populate mock students (HTTP Client disconnected)
                foreach (var courseRes in response)
                {
                    var dbCourse = courses.FirstOrDefault(c => c.Courseid == courseRes.CourseId);
                    if (dbCourse?.Enrollments != null)
                    {
                        courseRes.Students = dbCourse.Enrollments
                            .Select(e => new StudentInCourseResponse
                            {
                                StudentId = e.Studentid,
                                FullName = $"Mock Student {e.Studentid}",
                                Email = $"mock{e.Studentid}@example.com"
                            })
                            .ToList();
                    }
                }
            }

            var shapedData = response.SelectFields(query.Fields);

            return new ApiResponse<object>
            {
                success = true,
                message = "Get courses successfully",
                Data = shapedData,
                pagination = new PaginationMetadata
                {
                    Page = query.Page,
                    PageSize = query.Size,
                    TotalItems = totalItems,
                    TotalPages = (int)Math.Ceiling((double)totalItems / query.Size)
                }
            };
        }

        public async Task<CourseResponse?> GetByIdAsync(int id)
        {
            var course = await _unitOfWork.Courses.GetQueryable()
                .Include(x => x.Semester)
                .Include(x => x.Enrollments)
                .FirstOrDefaultAsync(x => x.Courseid == id);

            if (course == null)
            {
                return null;
            }

            var response = course.ToCourseResponse();

            // Populate Student details in course response (HTTP Client disconnected)
            response.Students = course.Enrollments
                .Select(e => new StudentInCourseResponse
                {
                    StudentId = e.Studentid,
                    FullName = $"Mock Student {e.Studentid}",
                    Email = $"mock{e.Studentid}@example.com"
                })
                .ToList();

            return response;
        }

        public async Task<ApiResponse<object>> GetEnrollmentsAsync(int courseId, QueryParameters query)
        {
            var enrollmentsQuery = _unitOfWork.Enrollments.GetQueryable()
                .Where(x => x.Courseid == courseId)
                .Expand(query)
                .Search(query);

            var totalItems = await enrollmentsQuery.CountAsync();
            var enrollments = await enrollmentsQuery.Sort(query).Paging(query).ToListAsync();
            var response = enrollments.Select(x => x.ToCourseEnrollmentResponse()).ToList();

            // Populate Student details inside enrollments (HTTP Client disconnected)
            foreach (var enrollRes in response)
            {
                enrollRes.Student = new StudentInEnrollmentResponse
                {
                    StudentId = enrollRes.StudentId,
                    FullName = $"Mock Student {enrollRes.StudentId}",
                    Email = $"mock{enrollRes.StudentId}@example.com"
                };
            }

            var shapedData = response.SelectFields(query.Fields);

            return new ApiResponse<object>
            {
                success = true,
                message = "Get courses enrollments successfull",
                Data = shapedData,
                pagination = new PaginationMetadata
                {
                    Page = query.Page,
                    PageSize = query.Size,
                    TotalItems = totalItems,
                    TotalPages = (int)Math.Ceiling((double)totalItems / query.Size)
                }
            };
        }

        public async Task<ApiResponse<CourseResponse>> UpdateAsync(int id, UpdateCourseRequest request)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(id);
            if (course == null)
            {
                return new ApiResponse<CourseResponse>
                {
                    success = false,
                    message = "Course not found"
                };
            }

            course.Coursename = request.CourseName;
            course.Semesterid = request.SemesterId;
            course.Coursecode = request.Coursecode;

            await _unitOfWork.Courses.UpdateAsync(course);
            await _unitOfWork.SaveChangesAsync();

            var courseFromDb = await _unitOfWork.Courses.GetQueryable()
                .Include(x => x.Semester)
                .Include(x => x.Enrollments)
                .FirstOrDefaultAsync(x => x.Courseid == course.Courseid);

            if (courseFromDb == null)
            {
                return new ApiResponse<CourseResponse>
                {
                    success = false,
                    message = "Course not found after update"
                };
            }

            var response = courseFromDb.ToCourseResponse();

            // Populate Student details (HTTP Client disconnected)
            response.Students = courseFromDb.Enrollments
                .Select(e => new StudentInCourseResponse
                {
                    StudentId = e.Studentid,
                    FullName = $"Mock Student {e.Studentid}",
                    Email = $"mock{e.Studentid}@example.com"
                })
                .ToList();

            return new ApiResponse<CourseResponse>
            {
                success = true,
                message = "Update course successfully",
                Data = response
            };
        }
    }
}
