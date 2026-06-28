using System.Collections.Generic;
using System.Linq;
using StudentService.Application.DTOs.Response;
using StudentService.Domain.Entities;

namespace StudentService.Application.Extensions
{
    public static class StudentMapperExtensions
    {
        public static StudentResponse ToStudentResponse(this Student student)
        {
            return new StudentResponse
            {
                StudentId = student.Studentid,
                FullName = student.Fullname,
                Email = student.Email,
                Age = student.Age,
                PhoneNumber = student.Phonenumber,
                StudentCode = student.Studentcode,
                DateOfBirth = student.Dateofbirth,
                Enrollments = new List<EnrollmentResponse>()
            };
        }

        public static List<StudentResponse> ToStudentResponseList(this IEnumerable<Student> students)
        {
            return students.Select(x => x.ToStudentResponse()).ToList();
        }
    }
}
