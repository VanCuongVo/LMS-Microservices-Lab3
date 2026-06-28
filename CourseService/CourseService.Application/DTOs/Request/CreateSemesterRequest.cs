using System;
using System.ComponentModel.DataAnnotations;

namespace CourseService.Application.DTOs.Request
{
    public class CreateSemesterRequest
    {
        [Required(ErrorMessage = "SemesterName is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "SemesterName must be between 2 and 100 characters")]
        public string SemesterName { get; set; } = string.Empty;

        [Required(ErrorMessage = "StartDate is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "EndDate is required")]
        public DateTime EndDate { get; set; }
    }
}
