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

        public StudentController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterStudent([FromBody] CreateStudentDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(new { error = "Email and Password are required." });

            if (await _context.Students.AnyAsync(s => s.Email == dto.Email))
                return BadRequest(new { error = "A student with that email already exists." });

            var student = new Student
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Password = dto.Password,
                GPA = dto.GPA,
                DesiredGPA = dto.DesiredGPA
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return Ok(student);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Email == loginDto.Email);
            if (student == null || student.Password != loginDto.Password)
                return Unauthorized(new { error = "Invalid credentials." });

            return Ok(student);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudentById(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();
            return Ok(student);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] UpdateStudentDto updatedStudentDto)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound($"No student found with ID = {id}");

            student.FirstName = updatedStudentDto.FirstName;
            student.LastName = updatedStudentDto.LastName;
            student.Email = updatedStudentDto.Email;
            student.Password = updatedStudentDto.Password;
            student.GPA = updatedStudentDto.GPA;
            student.DesiredGPA = updatedStudentDto.DesiredGPA;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/student/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound($"No student found with ID = {id}");

            // Remove all courses of the student first (optional, but good for referential integrity)
            var courses = _context.Courses.Where(c => c.StudentId == id);
            _context.Courses.RemoveRange(courses);

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent(); // 204 No Content
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
        public double DesiredGPA { get; set; }
    }
}
