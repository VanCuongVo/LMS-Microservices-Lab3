
using CourseService.Application.DTOs.Response;

namespace CourseService.Application.Interfaces
{
    public interface IStudentServiceClient
    {
        Task<StudentInEnrollmentResponse?> GetStudentByIdAsync(int studentId);
        Task<List<StudentInEnrollmentResponse>> GetStudentsByIdsAsync(IEnumerable<int> studentIds);
    }
}
