using System.Collections.Generic;

namespace CourseService.Application.DTOs.Response
{
    public class CourseResponse
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int SemesterId { get; set; }
        public string SemesterName { get; set; } = string.Empty;
        public List<EnrollmentInCourseResponse> Enrollments { get; set; } = new();
        public List<StudentInCourseResponse> Students { get; set; } = new();
    }
}
