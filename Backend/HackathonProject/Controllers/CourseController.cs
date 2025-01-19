using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        // GET: api/Courses/{studentId}
        [HttpGet("{studentId}")]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses(int studentId)
        {
            // Filter courses by the studentId and include student details if needed.
            var courses = await _context.Courses
                                        .Where(c => c.StudentId == studentId)
                                        .Include(c => c.Student)
                                        .ToListAsync();

            if (courses == null || !courses.Any())
            {
                return NotFound($"No courses found for student with ID {studentId}.");
            }

            return Ok(courses);
        }

        // POST: api/Courses
        [HttpPost]
        public async Task<ActionResult<Course>> AddCourse([FromBody] CreateCourseDto courseDto)
        {
            // Verify that the student exists
            if (!_context.Students.Any(s => s.StudentId == courseDto.StudentId))
            {
                return BadRequest("Invalid StudentId.");
            }

            // Map the DTO to a new Course entity
            var course = new Course
            {
                CourseName = courseDto.CourseName,
                Semester = courseDto.Semester,
                SectionId = courseDto.SectionId,
                Credits = courseDto.Credits,
                Grade = courseDto.Grade,
                StudentId = courseDto.StudentId
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCourses), new { studentId = course.StudentId }, course);
        }

        // PUT: api/Courses/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCourse(int id, [FromBody] UpdateCourseDto updatedCourseDto)
        {
            // Retrieve the existing course from the database
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            // Optional: Verify that the provided StudentId is valid
            if (!_context.Students.Any(s => s.StudentId == updatedCourseDto.StudentId))
            {
                return BadRequest("Invalid StudentId.");
            }

            // Map updated fields from the DTO to the course entity
            course.CourseName = updatedCourseDto.CourseName;
            course.Semester = updatedCourseDto.Semester;
            course.SectionId = updatedCourseDto.SectionId;
            course.Credits = updatedCourseDto.Credits;
            course.Grade = updatedCourseDto.Grade;
            course.StudentId = updatedCourseDto.StudentId;

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // DELETE: api/Courses/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent(); // Returns 204 No Content after successful deletion
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }
    }
}
