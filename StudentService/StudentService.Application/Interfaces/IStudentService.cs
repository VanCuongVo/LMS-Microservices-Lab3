using System.Threading.Tasks;
using StudentService.Application.DTOs.Request;
using StudentService.Application.DTOs.Response;

namespace StudentService.Application.Interfaces
{
    public interface IStudentService
    {
        Task<ApiResponse<object>> GetAllAsync(QueryParameters query);
        Task<StudentResponse?> GetByIdAsync(int id);
        Task<ApiResponse<StudentResponse>> CreateAsync(CreateStudentRequest request);
        Task<ApiResponse<StudentResponse>> UpdateAsync(int id, UpdateStudentRequest request);
        Task<ApiResponse<bool>> DeleteAsync(int id);
        Task<ApiResponse<List<StudentResponse>>> GetStudentsBatchAsync(System.Collections.Generic.IEnumerable<int> ids);
    }
}
