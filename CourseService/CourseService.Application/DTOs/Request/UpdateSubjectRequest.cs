using System.ComponentModel.DataAnnotations;

namespace CourseService.Application.DTOs.Request
{
    public class UpdateSubjectRequest
    {
        [Required(ErrorMessage = "SubjectCode is required")]
        [RegularExpression(@"^[A-Z]{3}\d{3}$", ErrorMessage = "SubjectCode must be like PRN232")]
        public string SubjectCode { get; set; } = null!;

        [Required(ErrorMessage = "SubjectName is required")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "SubjectName must be between 2 and 200 characters")]
        public string SubjectName { get; set; } = null!;

        [Range(1, 10, ErrorMessage = "Credit must be between 1 and 10")]
        public int Credit { get; set; }
    }
}
