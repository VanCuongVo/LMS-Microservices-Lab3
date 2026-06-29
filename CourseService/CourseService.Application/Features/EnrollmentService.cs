using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseService.Application.DTOs.Request;
using CourseService.Application.DTOs.Response;
using CourseService.Application.Extensions;
using CourseService.Application.Interfaces;
using CourseService.Application.Utility;
using CourseService.Domain.Entities;
using CourseService.Domain.IUnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CourseService.Application.Features
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStudentServiceClient _studentServiceClient;


        public EnrollmentService(IUnitOfWork unitOfWork, IStudentServiceClient studentServiceClient)
        {
            _unitOfWork = unitOfWork;
            _studentServiceClient = studentServiceClient;
        }

        public async Task<ApiResponse<EnrollmentResponse>> CreateAsync(CreateEnrollmentRequest request)
        {

            var student = await _studentServiceClient.GetStudentByIdAsync(request.StudentId);

            // Verify course exists
            var course = await _unitOfWork.Courses.GetByIdAsync(request.CourseId);
            if (course == null)
            {
                return new ApiResponse<EnrollmentResponse>
                {
                    success = false,
                    message = $"Course with id {request.CourseId} not found"
                };
            }

            var enrollment = new Enrollment
            {
                Studentid = request.StudentId,
                Courseid = request.CourseId,
                Enrolldate = DateTime.SpecifyKind(request.EnrollDate, DateTimeKind.Utc),
                Status = request.Status,
            };

            await _unitOfWork.Enrollments.AddAsync(enrollment);
            await _unitOfWork.SaveChangesAsync();

            var enrollmentFromDb = await _unitOfWork.Enrollments.GetQueryable()
                .Include(x => x.Course)
                .FirstAsync(x => x.Enrollmentid == enrollment.Enrollmentid);

            var response = enrollmentFromDb.ToEnrollmentResponse();

            response.Student = new StudentInEnrollmentResponse
            {
                StudentId = student.StudentId,
                FullName = student.FullName,
                Email = student.Email
            };
            return new ApiResponse<EnrollmentResponse>
            {
                success = true,
                message = "Create enrollment successfully",
                Data = response
            };
        }

        public async Task<ApiResponse<object>> GetAllAsync(QueryParameters query)
        {
            List<int>? matchingStudentIds = null;

            // Fetch matching student IDs if searching
            if (!string.IsNullOrEmpty(query.Search))
            {
                // HTTP Client disconnected - searching disabled
                matchingStudentIds = new List<int>();
            }

            var enrollmentQuery = _unitOfWork.Enrollments.GetQueryable()
                .Include(x => x.Course)
                .Search(query, matchingStudentIds)
                .Expand(query);

            var totalItems = await enrollmentQuery.CountAsync();
            var enrollments = await enrollmentQuery.Sort(query).Paging(query).ToListAsync();
            var response = enrollments.ToEnrollmentResponseList();



            // Populate Student details (HTTP Client disconnected - populating mock student details)

            foreach (var enrollRes in response)
            {
                var enrollEntity = enrollments.FirstOrDefault(e => e.Enrollmentid == enrollRes.EnrollmentId);
                if (enrollEntity != null)
                {
                    enrollRes.Student = new StudentInEnrollmentResponse
                    {
                        StudentId = enrollEntity.Studentid,
                        FullName = $"Mock Student {enrollEntity.Studentid}",
                        Email = $"mock{enrollEntity.Studentid}@example.com"
                    };
                }
            }

            var shapedData = response.SelectFields(query.Fields);

            return new ApiResponse<object>
            {
                success = true,
                message = "Get enrollments successfully",
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

        public async Task<EnrollmentResponse?> GetByIdAsync(int id)
        {
            var enrollment = await _unitOfWork.Enrollments.GetQueryable()
                .Include(x => x.Course)
                .FirstOrDefaultAsync(x => x.Enrollmentid == id);

            if (enrollment == null)
            {
                return null;
            }

            var response = enrollment.ToEnrollmentResponse();

            response.Student = new StudentInEnrollmentResponse
            {
                StudentId = enrollment.Studentid,
                FullName = $"Mock Student {enrollment.Studentid}",
                Email = $"mock{enrollment.Studentid}@example.com"
            };

            return response;
        }

        public async Task<ApiResponse<EnrollmentResponse?>> UpdateAsync(int id, UpdateEnrollmentRequest request)
        {
            var enrollment = await _unitOfWork.Enrollments.GetQueryable().FirstOrDefaultAsync(x => x.Enrollmentid == id);
            if (enrollment == null)
            {
                return new ApiResponse<EnrollmentResponse?>
                {
                    success = false,
                    message = $"Enrollment with id {id} not found",
                    Data = null
                };
            }

            // Verify student exists in StudentService (HTTP Client disconnected - mocking student check)
            var student = new StudentInEnrollmentResponse
            {
                StudentId = request.StudentId,
                FullName = $"Mock Student {request.StudentId}",
                Email = "mock@example.com"
            };

            // Verify course exists
            var course = await _unitOfWork.Courses.GetByIdAsync(request.CourseId);
            if (course == null)
            {
                return new ApiResponse<EnrollmentResponse?>
                {
                    success = false,
                    message = $"Course with id {request.CourseId} not found",
                    Data = null
                };
            }

            enrollment.Studentid = request.StudentId;
            enrollment.Courseid = request.CourseId;
            enrollment.Enrolldate = DateTime.SpecifyKind(request.EnrollDate, DateTimeKind.Utc);
            enrollment.Status = request.Status;

            await _unitOfWork.Enrollments.UpdateAsync(enrollment);
            await _unitOfWork.SaveChangesAsync();

            var enrollmentFromDb = await _unitOfWork.Enrollments.GetQueryable()
                .Include(x => x.Course)
                .FirstAsync(x => x.Enrollmentid == id);

            var response = enrollmentFromDb.ToEnrollmentResponse();
            response.Student = student;

            return new ApiResponse<EnrollmentResponse?>
            {
                success = true,
                message = "Update enrollment successfully",
                Data = response
            };
        }

        public async Task<ApiResponse<List<EnrollmentResponse>>> GetByStudentIdAsync(int studentId)
        {
            var enrollments = await _unitOfWork.Enrollments.GetQueryable()
                .Include(x => x.Course)
                .Where(x => x.Studentid == studentId)
                .ToListAsync();

            var responseList = enrollments.ToEnrollmentResponseList();

            return new ApiResponse<List<EnrollmentResponse>>
            {
                success = true,
                message = $"Get enrollments for student {studentId} successfully",
                Data = responseList
            };
        }
    }
}
