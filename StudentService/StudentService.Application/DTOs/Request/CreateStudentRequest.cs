using System;
using System.ComponentModel.DataAnnotations;
using StudentService.Application.Custom;

namespace StudentService.Application.DTOs.Request
{
    public class CreateStudentRequest
    {
        [Required(ErrorMessage = "FullName is required")]
        public required string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        public required string Email { get; set; }

        public DateTime DateOfBirth { get; set; }

        [Required]
        [Range(18, 60)]
        public int Age { get; set; }

        [Required]
        [Phone]
        public string Phonenumber { get; set; } = null!;

        [Required]
        [RegularExpression(@"^[A-Z]{2}\d{6}$", ErrorMessage = "StudentCode must be like SE198866")]
        [StudentCode(ErrorMessage = "Student code must be SE/CE/AI + 6 digits (ex: SE198866)")]
        public string Studentcode { get; set; } = null!;
    }
}
