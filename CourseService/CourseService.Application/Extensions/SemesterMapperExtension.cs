using System.Collections.Generic;
using System.Linq;
using CourseService.Application.DTOs.Response;
using CourseService.Domain.Entities;

namespace CourseService.Application.Extensions
{
    public static class SemesterMapperExtension
    {
        public static SemesterResponse ToSemesterReponse(this Semester semester)
        {
            return new SemesterResponse
            {
                SemesterId = semester.Semesterid,
                SemesterName = semester.Semestername,
                StartDate = semester.Startdate,
                EndDate = semester.Enddate,
                Courses = semester.Courses?.Select(x => new CourseResponse
                {
                    CourseId = x.Courseid,
                    CourseName = x.Coursename,
                    SemesterId = x.Semesterid,
                    SemesterName = semester.Semestername,
                    Enrollments = x.Enrollments?.Select(e => new EnrollmentInCourseResponse
                    {
                        EnrollmentId = e.Enrollmentid,
                        EnrollDate = e.Enrolldate,
                        Status = e.Status
                    }).ToList() ?? new List<EnrollmentInCourseResponse>(),
                    Students = new List<StudentInCourseResponse>() // populated by service layer if needed
                }).ToList() ?? new List<CourseResponse>()
            };
        }

        public static List<SemesterResponse> ToSemesterReponseList(this IEnumerable<Semester> semesters)
        {
            return semesters.Select(x => x.ToSemesterReponse()).ToList();
        }
    }
}
