using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HackathonProject.Models;

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
            if (string.IsNullOrWhiteSpace(newStudent.Email) || string.IsNullOrWhiteSpace(newStudent.Password))
            {
                return BadRequest(new { error = "Email and Password are required." });
            }

            bool emailExists = await _context.Students.AnyAsync(s => s.Email == newStudent.Email);
            if (emailExists)
            {
                return BadRequest(new { error = "A student with that email already exists." });
            }

            _context.Students.Add(newStudent);
            await _context.SaveChangesAsync();

            // Return only the student object as a flat JSON response
            return Ok(newStudent);
        }


        /// <summary>
        /// POST: api/student/login
        /// Validates credentials and returns the student object if successful.
        /// </summary>
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

            return Ok(new
            {
                student
            });
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

            return CreatedAtAction(nameof(GetStudentById), new { id = newStudent.StudentId }, newStudent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] UpdateStudentDto updatedStudentDto)
        {
            var existingStudent = await _context.Students.FindAsync(id);
            if (existingStudent == null)
            {
                return NotFound($"No student found with ID = {id}");
            }

            existingStudent.FirstName = updatedStudentDto.FirstName;
            existingStudent.LastName = updatedStudentDto.LastName;
            existingStudent.Email = updatedStudentDto.Email;
            existingStudent.Password = updatedStudentDto.Password;
            existingStudent.GPA = updatedStudentDto.GPA;

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
    }

    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UpdateStudentDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public double GPA { get; set; }
    }
}
