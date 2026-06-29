using Grpc.Core;
using StudentService.Application.Interfaces;
using StudentService.Grpc;

namespace StudentService.API.Services
{
    public class StudentGrpcService : StudentGrpc.StudentGrpcBase
    {
        private readonly IStudentService _studentService;

        public StudentGrpcService(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public override async Task<StudentResponse> GetStudentById(GetStudentRequest request, ServerCallContext context)
        {
            var student = await _studentService.GetByIdAsync(request.StudentId);
            if (student == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Student {request.StudentId} not found"));
            }

            return new StudentResponse
            {
                StudentId = student.StudentId,
                FullName = student.FullName ?? "",
                Email = student.Email ?? "",
                PhoneNumber = student.PhoneNumber ?? "",
                Age = student.Age,
                StudentCode = student.StudentCode ?? ""
            };
        }

        public override async Task<StudentsBatchResponse> GetStudentsByIds(GetStudentsByIdsRequest request, ServerCallContext context)
        {
            var result = await _studentService.GetStudentsBatchAsync(request.StudentIds);
            var response = new StudentsBatchResponse();

            if (result.success && result.Data != null)
            {
                response.Students.AddRange(result.Data.Select(s => new StudentResponse
                {
                    StudentId = s.StudentId,
                    FullName = s.FullName ?? "",
                    Email = s.Email ?? "",
                    PhoneNumber = s.PhoneNumber ?? "",
                    Age = s.Age,
                    StudentCode = s.StudentCode ?? ""
                }));
            }

            return response;
        }

        // Tương tự, bạn implement SearchStudents ở đây...
    }
}
