using System;
using StudentService.Domain.Enum;

namespace StudentService.Application.DTOs.Response
{
    public class EnrollmentResponse
    {
        public int EnrollmentId { get; set; }
        public DateTime EnrollDate { get; set; }
        public EnrollmentStatus Status { get; set; }
        public StudentInEnrollmentResponse? Student { get; set; }
        public CourseInEnrollmentResponse? Course { get; set; }
    }
}
