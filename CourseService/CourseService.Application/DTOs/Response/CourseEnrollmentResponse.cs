using System;
using CourseService.Domain.Enum;

namespace CourseService.Application.DTOs.Response
{
    public class CourseEnrollmentResponse
    {
        public int EnrollmentId { get; set; }
        public DateTime EnrollDate { get; set; }
        public EnrollmentStatus Status { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public StudentInEnrollmentResponse? Student { get; set; }
    }
}
