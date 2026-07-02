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
    public class SemestersService : ISemestersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStudentServiceClient _studentServiceClient;

        public SemestersService(IUnitOfWork unitOfWork, IStudentServiceClient studentServiceClient)
        {
            _unitOfWork = unitOfWork;
            _studentServiceClient = studentServiceClient;
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

            var studentIds = responses
                .Where(r => r.Courses != null)
                .SelectMany(r => r.Courses!)
                .Where(c => c.Enrollments != null)
                .SelectMany(c => c.Enrollments)
                .Select(e => e.StudentId)
                .ToList();

            var students = await _studentServiceClient.GetStudentsByIdsAsync(studentIds);
            var studentDict = students?.ToDictionary(s => s.StudentId) ?? new Dictionary<int, StudentInEnrollmentResponse>();

            foreach (var semResponse in responses)
            {
                if (semResponse.Courses == null) continue;
                foreach (var courseRes in semResponse.Courses)
                {
                    var dbCourse = semesters.SelectMany(s => s.Courses).FirstOrDefault(c => c.Courseid == courseRes.CourseId);
                    if (dbCourse?.Enrollments != null)
                    {

                        courseRes.Students = dbCourse.Enrollments
                            .Select(e =>
                            {

                                studentDict.TryGetValue(e.Studentid, out var student);
                                return new StudentInCourseResponse
                                {

                                    StudentId = e.Studentid,
                                    FullName = student.FullName,
                                    Email = student.Email
                                };
                            }).ToList();
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

            foreach (var courseRes in response.Courses ?? new List<CourseResponse>())
            {
                var dbCourse = semester.Courses
                    .FirstOrDefault(c => c.Courseid == courseRes.CourseId);

                if (dbCourse?.Enrollments == null)
                {
                    continue;
                }

                courseRes.Students = new List<StudentInCourseResponse>();

                foreach (var enrollment in dbCourse.Enrollments)
                {
                    try
                    {
                        var student = await _studentServiceClient
                            .GetStudentByIdAsync(enrollment.Studentid);

                        courseRes.Students.Add(new StudentInCourseResponse
                        {
                            StudentId = student.StudentId,
                            FullName = student.FullName,
                            Email = student.Email
                        });
                    }
                    catch
                    {
                        courseRes.Students.Add(new StudentInCourseResponse
                        {
                            StudentId = enrollment.Studentid,
                            FullName = $"Unknown Student {enrollment.Studentid}",
                            Email = string.Empty
                        });
                    }
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
