namespace HackathonProject.Models
{
    public class CreateCourseDto
    {
        public string CourseName { get; set; }
        public string Semester { get; set; }
        public int SectionId { get; set; }
        public int Credits { get; set; }
        public double Grade { get; set; }
        public int StudentId { get; set; }
    }
}
