using System.ComponentModel.DataAnnotations;

namespace HackathonProject.Models
{
    public class CreateCourseDto
    {
        [Required]
        public string CourseName { get; set; }

        [Required]
        public string Semester { get; set; }

        public int SectionId { get; set; }
        public int Credits { get; set; }
        public double Grade { get; set; }
        public int StudentId { get; set; }
    }
}
