using System.Threading.Tasks;
using CourseService.Application.DTOs.Request;
using CourseService.Application.DTOs.Response;

namespace CourseService.Application.Interfaces
{
    public interface IEnrollmentService
    {
        Task<ApiResponse<object>> GetAllAsync(QueryParameters query);
        Task<EnrollmentResponse?> GetByIdAsync(int id);
        Task<ApiResponse<List<EnrollmentResponse>>> GetByStudentIdAsync(int studentId);
        Task<ApiResponse<EnrollmentResponse>> CreateAsync(CreateEnrollmentRequest request);
        Task<ApiResponse<EnrollmentResponse?>> UpdateAsync(int id, UpdateEnrollmentRequest request);
    }
}
