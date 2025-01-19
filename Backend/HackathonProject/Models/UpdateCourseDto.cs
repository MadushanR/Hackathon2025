namespace HackathonProject.Models
{
    public class UpdateCourseDto
    {
        

        // Fields that can be updated
        public string CourseName { get; set; }
        public string Semester { get; set; }
        public int SectionId { get; set; }
        public int Credits { get; set; }
        public double Grade { get; set; }

        // StudentId may be updatable if you allow changing the course's associated student,
        // but in many cases this might be kept immutable. Adjust as needed.
        public int StudentId { get; set; }
    }
}
