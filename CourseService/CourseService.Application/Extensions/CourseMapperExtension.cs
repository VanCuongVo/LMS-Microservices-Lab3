using System.Collections.Generic;
using System.Linq;
using CourseService.Application.DTOs.Response;
using CourseService.Domain.Entities;

namespace CourseService.Application.Extensions
{
    public static class CourseMapperExtension
    {
        public static CourseEnrollmentResponse ToCourseEnrollmentResponse(this Enrollment enrollment)
        {
            return new CourseEnrollmentResponse
            {
                EnrollmentId = enrollment.Enrollmentid,
                EnrollDate = enrollment.Enrolldate,
                Status = enrollment.Status,
                StudentId = enrollment.Studentid,
                CourseId = enrollment.Courseid,
                Student = null // to be populated by service layer
            };
        }

        public static CourseResponse ToCourseResponse(this Course course)
        {
            return new CourseResponse
            {
                CourseId = course.Courseid,
                CourseName = course.Coursename,
                SemesterId = course.Semester?.Semesterid ?? 0,
                SemesterName = course.Semester?.Semestername ?? string.Empty,
                Enrollments = course.Enrollments?.Select(x => new EnrollmentInCourseResponse
                {
                    EnrollmentId = x.Enrollmentid,
                    EnrollDate = x.Enrolldate,
                    Status = x.Status
                }).ToList() ?? new List<EnrollmentInCourseResponse>(),
                Students = new List<StudentInCourseResponse>() // to be populated by service layer
            };
        }

        public static List<CourseResponse> ToCourseResponseList(this IEnumerable<Course> courses)
        {
            return courses.Select(ToCourseResponse).ToList();
        }
    }
}
