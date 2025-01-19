using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HackathonProject.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Semester { get; set; }
        public int SectionId { get; set; }
        public int Credits { get; set; }
        public double Grade { get; set; }

        // Foreign Key
        public int StudentId { get; set; }

        // Navigation Property
        [ForeignKey("StudentId")]
        public Student Student { get; set; }
    }
}
