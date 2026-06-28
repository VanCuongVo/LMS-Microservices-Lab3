using System.Collections.Generic;
using System.Linq;
using CourseService.Application.DTOs.Response;
using CourseService.Domain.Entities;

namespace CourseService.Application.Extensions
{
    public static class SubjectMapperExtensions
    {
        public static SubjectResponse ToSubjectResponse(this Subject subject)
        {
            return new SubjectResponse
            {
                SubjectId = subject.Subjectid,
                SubjectName = subject.Subjectname,
                SubjectCode = subject.Subjectcode,
                Credit = subject.Credit
            };
        }

        public static List<SubjectResponse> ToSubjectResponseList(this IEnumerable<Subject> subjects)
        {
            return subjects.Select(x => x.ToSubjectResponse()).ToList();
        }
    }
}
