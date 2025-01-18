using HackathonProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            // Attempt to find a matching student by email
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Email == loginDto.Email);

            if (student == null || student.Password != loginDto.Password)
            {
                return Unauthorized("Invalid credentials");
            }

            // Generate a session if time permits
            return Ok(new { message = "Login successful", studentId = student.StudentId });
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
            existingStudent.Name = updatedStudentDto.Name;
            existingStudent.Email = updatedStudentDto.Email;
            existingStudent.Password = updatedStudentDto.Password;
            existingStudent.SchoolName = updatedStudentDto.SchoolName;
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

    }
}
