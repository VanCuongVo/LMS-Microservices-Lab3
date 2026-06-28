using System;
using System.Collections.Generic;

namespace CourseService.Application.DTOs.Response
{
    public class SemesterResponse
    {
        public int SemesterId { get; set; }
        public string SemesterName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<CourseResponse>? Courses { get; set; }
    }
}
