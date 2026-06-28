using System;
using CourseService.Domain.Enum;

namespace CourseService.Domain.Entities
{
    public class Enrollment
    {
        public int Enrollmentid { get; set; }
        public int Studentid { get; set; }
        public int Courseid { get; set; }
        public DateTime Enrolldate { get; set; }
        public EnrollmentStatus Status { get; set; }

        // Navigation
        public virtual Course Course { get; set; } = null!;
    }
}
