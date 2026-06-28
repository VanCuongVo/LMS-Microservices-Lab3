using System;
using System.Collections.Generic;

namespace CourseService.Domain.Entities
{
    public class Semester
    {
        public int Semesterid { get; set; }
        public string Semestername { get; set; } = null!;
        public DateTime Startdate { get; set; }
        public DateTime Enddate { get; set; }

        // Navigation
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
