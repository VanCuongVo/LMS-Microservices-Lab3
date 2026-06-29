using CourseService.Application.DTOs.Response;
using CourseService.Application.Interfaces;
using StudentService.Grpc;

namespace CourseService.Infrastructure.Services
{
    public class StudentGrpcClient : IStudentServiceClient
    {

        private readonly StudentGrpc.StudentGrpcClient _grpcClient;

        public StudentGrpcClient(StudentGrpc.StudentGrpcClient grpcClient)
        {
            _grpcClient = grpcClient;
        }

        public async Task<StudentInEnrollmentResponse?> GetStudentByIdAsync(int studentId)
        {
            try
            {
                var request = new GetStudentRequest
                {
                    StudentId = studentId
                };
                var response = await _grpcClient.GetStudentByIdAsync(request);

                return new StudentInEnrollmentResponse
                {
                    StudentId = response.StudentId,
                    FullName = response.FullName,
                    Email = response.Email,
                    PhoneNumber = response.PhoneNumber,
                    Age = response.Age,
                    StudentCode = response.StudentCode
                };

            }
            catch
            {

                return null;
            }
        }

        public async Task<List<StudentInEnrollmentResponse>> GetStudentsByIdsAsync(IEnumerable<int> studentIds)
        {
            try
            {
                var request = new GetStudentsByIdsRequest();
                request.StudentIds.AddRange(studentIds);
                var response = await _grpcClient.GetStudentsByIdsAsync(request);

                return response.Students.Select(s => new StudentInEnrollmentResponse
                {
                    StudentId = s.StudentId,
                    FullName = s.FullName,
                    Email = s.Email,
                    PhoneNumber = s.PhoneNumber,
                    Age = s.Age,
                    StudentCode = s.StudentCode
                }).ToList();
            }
            catch
            {

                return null;
            }
        }
    }
}