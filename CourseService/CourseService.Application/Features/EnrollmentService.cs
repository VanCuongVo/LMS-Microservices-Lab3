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
            if (student == null)
            {
                return new ApiResponse<EnrollmentResponse>
                {
                    success = false,
                    message = $"Student with id {request.StudentId} not found"
                };
            }

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
            var enrollmentQuery = _unitOfWork.Enrollments.GetQueryable()
                .Include(x => x.Course)
                .Search(query)
                .Expand(query);

            var totalItems = await enrollmentQuery.CountAsync();
            var enrollments = await enrollmentQuery.Sort(query).Paging(query).ToListAsync();
            var response = enrollments.ToEnrollmentResponseList();

            foreach (var enrollRes in response)
            {
                var enrollEntity = enrollments.FirstOrDefault(e => e.Enrollmentid == enrollRes.EnrollmentId);
                var student = await _studentServiceClient.GetStudentByIdAsync(enrollEntity.Studentid);
                if (student != null)
                {
                    enrollRes.Student = new StudentInEnrollmentResponse
                    {
                        StudentId = student.StudentId,
                        FullName = student.FullName,
                        Email = student.Email
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
            var student = await _studentServiceClient.GetStudentByIdAsync(enrollment.Studentid);

            if (student != null)
            {
                response.Student = new StudentInEnrollmentResponse
                {
                    StudentId = student.StudentId,
                    FullName = student.FullName,
                    Email = student.Email
                };
            }
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

            // Verify student exists in StudentService 
            var student = await _studentServiceClient.GetStudentByIdAsync(request.StudentId);
            if (student == null)
            {
                return new ApiResponse<EnrollmentResponse?>
                {
                    success = false,
                    message = $"Student with id {request.StudentId} not found",
                    Data = null
                };
            }

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
            response.Student = new StudentInEnrollmentResponse
            {
                StudentId = student.StudentId,
                FullName = student.FullName,
                Email = student.Email
            };

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
