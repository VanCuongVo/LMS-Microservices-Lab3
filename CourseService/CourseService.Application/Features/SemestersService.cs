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
    public class SemestersService : ISemestersService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SemestersService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<SemesterResponse>> CreateAsync(CreateSemesterRequest request)
        {
            var semester = new Semester
            {
                Semestername = request.SemesterName,
                Enddate = DateTime.SpecifyKind(request.EndDate, DateTimeKind.Utc),
                Startdate = DateTime.SpecifyKind(request.StartDate, DateTimeKind.Utc),
            };

            await _unitOfWork.Semesters.AddAsync(semester);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<SemesterResponse>
            {
                success = true,
                message = "Create semester successfully",
                Data = semester.ToSemesterReponse()
            };
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var semester = await _unitOfWork.Semesters.GetByIdAsync(id);
            if (semester != null)
            {
                await _unitOfWork.Semesters.DeleteAsync(semester.Semesterid);
                await _unitOfWork.SaveChangesAsync();
                return new ApiResponse<bool>
                {
                    success = true,
                    message = "Delete semester successfully",
                    Data = true
                };
            }

            return new ApiResponse<bool>
            {
                success = false,
                message = "Delete semester fails",
                Data = false
            };
        }

        public async Task<ApiResponse<object>> GetAllAsync(QueryParameters query)
        {
            var semesterQuery = _unitOfWork.Semesters.GetQueryable()
                .Search(query)
                .Expand(query);

            // If expand contains course, eager load it with enrollments
            if (!string.IsNullOrEmpty(query.Expand) && query.Expand.Split(',').Any(x => x.Trim().Equals("course", StringComparison.OrdinalIgnoreCase)))
            {
                semesterQuery = semesterQuery.Include(s => s.Courses).ThenInclude(c => c.Enrollments);
            }

            var totalItems = await semesterQuery.CountAsync();
            var semesters = await semesterQuery.Sort(query).Paging(query).ToListAsync();
            var responses = semesters.ToSemesterReponseList();

            // Populate Student details in CourseResponse if needed
            var studentIds = responses
                .Where(r => r.Courses != null)
                .SelectMany(r => r.Courses!)
                .Where(c => c.Enrollments != null)
                .SelectMany(c => c.Enrollments)
                .Select(e => e.EnrollmentId) // Wait, we need StudentIds from Course's DB enrollments, but responses.Courses has only EnrollmentInCourseResponse which does not have StudentId!
                // Ah! We can collect StudentIds directly from the database entity list 'semesters'!
                .ToList();

            // Let's gather student IDs from Entity list
            var dbStudentIds = semesters
                .SelectMany(s => s.Courses)
                .SelectMany(c => c.Enrollments)
                .Select(e => e.Studentid)
                .Distinct()
                .ToList();

            // Populate mock students (HTTP Client disconnected)
            foreach (var semResponse in responses)
            {
                if (semResponse.Courses == null) continue;
                foreach (var courseRes in semResponse.Courses)
                {
                    var dbCourse = semesters.SelectMany(s => s.Courses).FirstOrDefault(c => c.Courseid == courseRes.CourseId);
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

            var shapedData = responses.SelectFields(query.Fields);

            return new ApiResponse<object>
            {
                success = true,
                message = "Get semester successfully",
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

        public async Task<SemesterResponse?> GetByIdAsync(int id)
        {
            var semester = await _unitOfWork.Semesters.GetQueryable()
                .Include(s => s.Courses)
                .ThenInclude(c => c.Enrollments)
                .FirstOrDefaultAsync(s => s.Semesterid == id);

            if (semester == null)
            {
                return null;
            }

            var response = semester.ToSemesterReponse();

            // Populate Student details
            var dbStudentIds = semester.Courses
                .SelectMany(c => c.Enrollments)
                .Select(e => e.Studentid)
                .Distinct()
                .ToList();

            // Populate mock students (HTTP Client disconnected)
            foreach (var courseRes in response.Courses ?? new List<CourseResponse>())
            {
                var dbCourse = semester.Courses.FirstOrDefault(c => c.Courseid == courseRes.CourseId);
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

            return response;
        }

        public async Task<ApiResponse<SemesterResponse>> UpdateAsync(int id, UpdateSemesterRequest request)
        {
            var semester = await _unitOfWork.Semesters.GetByIdAsync(id);
            if (semester == null)
            {
                return new ApiResponse<SemesterResponse>
                {
                    success = false,
                    message = "Semester not found"
                };
            }

            semester.Semestername = request.SemesterName;
            semester.Enddate = DateTime.SpecifyKind(request.EndDate, DateTimeKind.Utc);
            semester.Startdate = DateTime.SpecifyKind(request.StartDate, DateTimeKind.Utc);

            await _unitOfWork.Semesters.UpdateAsync(semester);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<SemesterResponse>
            {
                success = true,
                message = "Update semester successfully",
                Data = semester.ToSemesterReponse()
            };
        }
    }
}
