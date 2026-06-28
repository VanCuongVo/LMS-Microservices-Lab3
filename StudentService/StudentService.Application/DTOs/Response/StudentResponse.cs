using System;
using System.Collections.Generic;

namespace StudentService.Application.DTOs.Response
{
    public class StudentResponse
    {
        public int StudentId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int Age { get; set; }
        public string? StudentCode { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ICollection<EnrollmentResponse>? Enrollments { get; set; }
    }
}
