using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HackathonProject.Models;

namespace HackathonProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly DataContext _context;

        public CourseController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("{studentId}")]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses(int studentId)
        {
            var courses = await _context.Courses.Where(c => c.StudentId == studentId).ToListAsync();
            if (!courses.Any())
                return Ok(new List<Course>()); // return empty list instead of 404
            return Ok(courses);
        }


        [HttpPost]
        public async Task<ActionResult<Course>> AddCourse([FromBody] CreateCourseDto courseDto)
        {
            if (!_context.Students.Any(s => s.StudentId == courseDto.StudentId))
                return BadRequest("Invalid StudentId.");

            // Semester must not be empty
            if (string.IsNullOrWhiteSpace(courseDto.Semester))
                return BadRequest("Semester is required.");

            var course = new Course
            {
                CourseName = courseDto.CourseName,
                Semester = courseDto.Semester,  // fix: assign Semester
                SectionId = courseDto.SectionId,
                Credits = courseDto.Credits,
                Grade = courseDto.Grade,
                StudentId = courseDto.StudentId
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCourses), new { studentId = course.StudentId }, course);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> EditCourse(int id, [FromBody] UpdateCourseDto updatedCourseDto)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            if (!_context.Students.Any(s => s.StudentId == updatedCourseDto.StudentId))
                return BadRequest("Invalid StudentId.");

            course.CourseName = updatedCourseDto.CourseName;
            course.Semester = updatedCourseDto.Semester;
            course.SectionId = updatedCourseDto.SectionId;
            course.Credits = updatedCourseDto.Credits;
            course.Grade = updatedCourseDto.Grade;
            course.StudentId = updatedCourseDto.StudentId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
