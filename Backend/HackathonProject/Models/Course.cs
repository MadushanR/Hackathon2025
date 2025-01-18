using System.ComponentModel.DataAnnotations;

namespace HackathonProject.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; } 
        public string CourseName { get; set; } 
        public int SectionId { get; set; } 
        public int Credits { get; set; }
        public decimal Grade { get; set; }

        // Navigation Property
        public ICollection<Student> Students { get; set; }
    }
}
