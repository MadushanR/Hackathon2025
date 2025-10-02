using System.ComponentModel.DataAnnotations;

namespace HackathonProject.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public double GPA { get; set; }

        // Add DesiredGPA to support frontend
        public double DesiredGPA { get; set; }

        // Navigation property
        public ICollection<Course> Courses { get; set; }
    }
}
