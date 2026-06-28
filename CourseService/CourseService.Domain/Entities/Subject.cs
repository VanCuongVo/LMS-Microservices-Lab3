namespace CourseService.Domain.Entities
{
    public class Subject
    {
        public int Subjectid { get; set; }
        public string Subjectcode { get; set; } = null!;
        public string Subjectname { get; set; } = null!;
        public int Credit { get; set; }
    }
}
