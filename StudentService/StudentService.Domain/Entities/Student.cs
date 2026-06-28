using System;

namespace StudentService.Domain.Entities
{
    public class Student
    {
        public int Studentid { get; set; }

        public string Fullname { get; set; } = null!;

        public string Email { get; set; } = null!;

        public DateTime Dateofbirth { get; set; }

        public string Studentcode { get; set; } = null!;

        public int Age { get; set; }

        public string Phonenumber { get; set; } = null!;
    }
}
