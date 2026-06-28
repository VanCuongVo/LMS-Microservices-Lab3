using System.Collections.Generic;
using System.Threading.Tasks;
using CourseService.Application.DTOs.Request;
using CourseService.Application.DTOs.Response;

namespace CourseService.Application.Interfaces
{
    public interface ISubjectService
    {
        Task<ApiResponse<IEnumerable<SubjectResponse>>> GetAllAsync(QueryParameters query);
        Task<SubjectResponse?> GetByIdAysnc(int id);
        Task<ApiResponse<SubjectResponse>> CreateAsync(CreateSubjectRequest request);
        Task<ApiResponse<SubjectResponse>> UpdateAsync(int id, UpdateSubjectRequest request);
        Task<ApiResponse<bool>> DeleteAsync(int id);
    }
}
