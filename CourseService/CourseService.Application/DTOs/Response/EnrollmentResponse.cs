using System;
using CourseService.Domain.Enum;

namespace CourseService.Application.DTOs.Response
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
