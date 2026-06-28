using System.Collections.Generic;

namespace CourseService.Domain.Entities
{
    public class Course
    {
        public int Courseid { get; set; }
        public string Coursename { get; set; } = null!;
        public int Semesterid { get; set; }
        public string Coursecode { get; set; } = null!;

        // Navigation
        public virtual Semester Semester { get; set; } = null!;
        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
