using System.Threading.Tasks;
using CourseService.Application.DTOs.Request;
using CourseService.Application.DTOs.Response;

namespace CourseService.Application.Interfaces
{
    public interface ISemestersService
    {
        Task<ApiResponse<object>> GetAllAsync(QueryParameters query);
        Task<SemesterResponse?> GetByIdAsync(int id);
        Task<ApiResponse<SemesterResponse>> CreateAsync(CreateSemesterRequest request);
        Task<ApiResponse<SemesterResponse>> UpdateAsync(int id, UpdateSemesterRequest request);
        Task<ApiResponse<bool>> DeleteAsync(int id);
    }
}
