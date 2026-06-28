using System.Collections.Generic;
using System.Threading.Tasks;
using StudentService.Application.DTOs.Response;

namespace StudentService.Application.Interfaces
{
    public interface ICourseServiceClient
    {
        Task<List<EnrollmentResponse>> GetEnrollmentsByStudentIdAsync(int studentId);
    }
}
