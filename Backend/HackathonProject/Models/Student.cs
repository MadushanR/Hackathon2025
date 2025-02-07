﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public double DesiredGPA { get; set; }

        // Navigation Property for many courses
        public ICollection<Course> Courses { get; set; } = new List<Course>();

    }
}
    