using HackathonProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text;

namespace HackathonProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;


        public StudentController(DataContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterStudent([FromBody] Student newStudent)
        {
            // 1. Basic validation: check if essential fields are provided
            //    (Alternatively, use ModelState or data annotations)
            if (string.IsNullOrWhiteSpace(newStudent.Email) ||
                string.IsNullOrWhiteSpace(newStudent.Password))
            {
                return BadRequest(new
                {
                    error = "Email and Password are required."
                });
            }

            // 2. Check if email already exists in DB
            bool emailExists = await _context.Students
                .AnyAsync(s => s.Email == newStudent.Email);

            if (emailExists)
            {
                // Return a 400 with a JSON body
                return BadRequest(new
                {
                    error = "A student with that email already exists."
                });
            }

            // 3. Create the new student record
            _context.Students.Add(newStudent);
            await _context.SaveChangesAsync();

            // 4. Return 201 Created with the newly created student in the response
            return CreatedAtAction(
                nameof(GetStudentById),
                new { id = newStudent.StudentId },
                new
                {
                    student = newStudent
                }
            );
        }

        // ----------------------------
        // POST: api/student/login
        // ----------------------------
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (string.IsNullOrWhiteSpace(loginDto.Email) || string.IsNullOrWhiteSpace(loginDto.Password))
            {
                return BadRequest(new { error = "Email and Password are required." });
            }

            var student = await _context.Students.FirstOrDefaultAsync(s => s.Email == loginDto.Email);
            if (student == null || student.Password != loginDto.Password)
            {
                return Unauthorized(new { error = "Invalid credentials. Please check your email and password." });
            }

            // Return the student object directly
            return Ok(student);
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetAllStudents()
        {
            var students = await _context.Students.ToListAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudentById(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound($"No student found with ID = {id}");
            }

            return Ok(student);
        }

        [HttpPost]
        public async Task<ActionResult<Student>> CreateStudent([FromBody] Student newStudent)
        {
            _context.Students.Add(newStudent);
            await _context.SaveChangesAsync();

            // Returns a 201 Created response along with the newly created resource location
            return CreatedAtAction(nameof(GetStudentById), new { id = newStudent.StudentId }, newStudent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] UpdateStudentDto updatedStudentDto)
        {
            // Check if the student exists
            var existingStudent = await _context.Students.FindAsync(id);
            if (existingStudent == null)
            {
                return NotFound($"No student found with ID = {id}");
            }

            // Update the allowed fields
            existingStudent.FirstName = updatedStudentDto.FirstName;
            existingStudent.LastName = updatedStudentDto.LastName;
            existingStudent.Email = updatedStudentDto.Email;
            existingStudent.Password = updatedStudentDto.Password;
            existingStudent.GPA = updatedStudentDto.GPA;

            // Save changes to the database
            await _context.SaveChangesAsync();

            return NoContent(); // Return 204 No Content
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound($"No student found with ID = {id}");
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{studentId}/gpa")]
        public async Task<IActionResult> CalculateGpaBySemester(int studentId)
        {
            // 1. Get all courses for the student
            var courses = await _context.Courses
                .Where(c => c.StudentId == studentId)
                .ToListAsync();

            if (courses == null || !courses.Any())
            {
                return NotFound(new { error = "No courses found for this student." });
            }

            // 2. Group courses by semester
            var gpaBySemester = courses
                .GroupBy(c => c.Semester)
                .Select(group => new
                {
                    Semester = group.Key,
                    GPA = CalculateGpa(group.ToList())
                })
                .ToList();

            return Ok(gpaBySemester);
        }

        /// <summary>
        /// Helper method to calculate GPA for a list of courses.
        /// </summary>
        private double CalculateGpa(List<Course> courses)
        {
            double totalGradePoints = 0;
            int totalCredits = 0;

            foreach (var course in courses)
            {
                totalGradePoints += course.Grade * course.Credits;
                totalCredits += course.Credits;
            }

            return totalCredits > 0 ? totalGradePoints / totalCredits : 0;
        }

    }

}

