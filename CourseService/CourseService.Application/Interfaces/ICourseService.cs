using System.Threading.Tasks;
using CourseService.Application.DTOs.Request;
using CourseService.Application.DTOs.Response;

namespace CourseService.Application.Interfaces
{
    public interface ICourseService
    {
        Task<ApiResponse<object>> GetAllAsync(QueryParameters query);
        Task<CourseResponse?> GetByIdAsync(int id);
        Task<ApiResponse<CourseResponse>> CreateAsync(CreateCourseRequest request);
        Task<ApiResponse<CourseResponse>> UpdateAsync(int id, UpdateCourseRequest request);
        Task<ApiResponse<bool>> DeleteAsync(int id);
        Task<ApiResponse<object>> GetEnrollmentsAsync(int courseId, QueryParameters query);
    }
}
