using System.Collections.Generic;
using System.Linq;
using CourseService.Application.DTOs.Response;
using CourseService.Domain.Entities;

namespace CourseService.Application.Extensions
{
    public static class EnrollmentMapperExtension
    {
        public static EnrollmentResponse ToEnrollmentResponse(this Enrollment enrollment)
        {
            return new EnrollmentResponse
            {
                EnrollmentId = enrollment.Enrollmentid,
                EnrollDate = enrollment.Enrolldate,
                Status = enrollment.Status,
                Student = null, // to be populated by service layer
                Course = new CourseInEnrollmentResponse
                {
                    CourseId = enrollment.Course?.Courseid ?? enrollment.Courseid,
                    CourseName = enrollment.Course?.Coursename ?? string.Empty
                }
            };
        }

        public static List<EnrollmentResponse> ToEnrollmentResponseList(this IEnumerable<Enrollment> enrollments)
        {
            return enrollments.Select(x => x.ToEnrollmentResponse()).ToList();
        }
    }
}
