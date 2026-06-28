using System;
using System.ComponentModel.DataAnnotations;
using CourseService.Domain.Enum;

namespace CourseService.Application.DTOs.Request
{
    public class CreateEnrollmentRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "StudentId must be a positive integer")]
        public int StudentId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "CourseId must be a positive integer")]
        public int CourseId { get; set; }

        public DateTime EnrollDate { get; set; }

        [EnumDataType(typeof(EnrollmentStatus))]
        public EnrollmentStatus Status { get; set; }
    }
}
