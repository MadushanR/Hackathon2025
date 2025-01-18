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
    public class CoursesController : ControllerBase
    {
        private readonly DataContext _context;

        public CoursesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Courses/{studentId}
        [HttpGet("{studentId}")]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses(int studentId)
        {
            // Filter courses by the studentId
            var courses = await _context.Courses
                                        .Where(c => c.StudentId == studentId)  // Filter by studentId
                                        .Include(c => c.Student)  // Include student details
                                        .ToListAsync();

            // If no courses found, return NotFound
            if (courses == null || !courses.Any())
            {
                return NotFound($"No courses found for student with ID {studentId}.");
            }

            // Return the courses for the specified student
            return Ok(courses); // Use Ok() to return the list of courses
        }

        // POST: api/Courses
        [HttpPost]
        public async Task<ActionResult<Course>> AddCourse(Course course)
        {
            if (!_context.Students.Any(s => s.StudentId == course.StudentId))
            {
                return BadRequest("Invalid StudentId.");
            }

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            // Use GetCourses instead of GetCourse to match the method
            return CreatedAtAction(nameof(GetCourses), new { studentId = course.StudentId }, course);
        }

        // PUT: api/Courses/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCourse(int id, Course course)
        {
            if (id != course.CourseId)
            {
                return BadRequest();
            }

            if (!_context.Students.Any(s => s.StudentId == course.StudentId))
            {
                return BadRequest("Invalid StudentId.");
            }

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

            return NoContent(); // Returns No Content if update is successful
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

            return NoContent(); // Returns No Content after successful deletion
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }
    }
}
