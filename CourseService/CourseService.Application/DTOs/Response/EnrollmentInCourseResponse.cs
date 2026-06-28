using System;
using CourseService.Domain.Enum;

namespace CourseService.Application.DTOs.Response
{
    public class EnrollmentInCourseResponse
    {
        public int EnrollmentId { get; set; }
        public DateTime EnrollDate { get; set; }
        public EnrollmentStatus Status { get; set; }
    }
}
