using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CourseService.Application.Custom
{
    public class CoursecodeAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null) return false;

            string code = value.ToString()!.Trim();

            return Regex.IsMatch(code, @"^[A-Z]{2,4}\d{3,4}$");
        }
    }
}
