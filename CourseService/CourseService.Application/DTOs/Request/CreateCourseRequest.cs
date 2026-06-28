using System.ComponentModel.DataAnnotations;
using CourseService.Application.Custom;

namespace CourseService.Application.DTOs.Request
{
    public class CreateCourseRequest
    {
        [Required(ErrorMessage = "CourseName is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "CourseName must be between 2 and 100 characters")]
        public string CourseName { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "SemesterId must be a positive integer")]
        public int SemesterId { get; set; }

        [Required(ErrorMessage = "CourseCode is required")]
        [RegularExpression(@"^[A-Z]{3}\d{3}$", ErrorMessage = "CourseCode must be like PRN232")]
        [Coursecode]
        public string Coursecode { get; set; } = null!;
    }
}
