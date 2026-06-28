using System.Collections.Generic;
using System.Threading.Tasks;
using CourseService.Application.DTOs.Response;

namespace CourseService.Application.Interfaces
{
    public interface IStudentServiceClient
    {
        Task<StudentInEnrollmentResponse?> GetStudentByIdAsync(int studentId);
        Task<List<StudentInEnrollmentResponse>> GetStudentsByIdsAsync(IEnumerable<int> studentIds);
        Task<List<StudentInEnrollmentResponse>> SearchStudentsAsync(string searchTerm);
    }
}
