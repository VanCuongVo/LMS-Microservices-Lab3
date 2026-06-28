namespace CourseService.Application.DTOs.Response
{
    public class StudentInEnrollmentResponse
    {
        public int StudentId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public int? Age { get; set; }
        public string? StudentCode { get; set; }
    }
}
