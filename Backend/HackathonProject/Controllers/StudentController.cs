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
        /// <summary>
        /// POST: api/student/generate-plan
        /// Generates an academic plan using OpenAI based on the provided GPA and credit information.
        /// </summary>
    //     [HttpPost("generate-plan")]
    //     public async Task<IActionResult> GeneratePlan([FromBody] PlanRequestDto dto)
    //     {
    //         // 1. Check if the student exists
    //         var student = await _context.Students.FindAsync(dto.StudentId);
    //         if (student == null)
    //         {
    //             return NotFound($"No student found with ID = {dto.StudentId}");
    //         }

    //         // 2. Build an OpenAI prompt
    //         string prompt = $"A student has a current GPA of {dto.CurrentGPA} " +
    //                         $"and wants to achieve a GPA of {dto.DesiredGPA}. " +
    //                         $"They need {dto.TotalCreditsNeeded} more credits to graduate. " +
    //                         $"Provide a recommended academic plan including course suggestions and study strategies.";

    //         // 3. Call OpenAI
    //         string openAiResponse = await CallOpenAIAsync(prompt);

    //         // 4. Return the AI-generated text directly
    //         return Ok(new { planText = openAiResponse });
    //     }

    //     /// <summary>
    //     /// Helper method to call OpenAI API.
    //     /// </summary>
    //     private async Task<string> CallOpenAIAsync(string prompt)
    //     {
    //         // 1. Retrieve API key from config
    //         string apiKey = _config["OpenAI:ApiKey"];
    //         if (string.IsNullOrEmpty(apiKey))
    //         {
    //             return "OpenAI API Key not configured.";
    //         }

    //         // 2. Create HttpClient
    //         using var client = new HttpClient();
    //         client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

    //         // 3. Build request body
    //         var requestBody = new
    //         {
    //             model = "text-davinci-003",
    //             prompt = prompt,
    //             max_tokens = 150,
    //             temperature = 0.7
    //         };

    //         // 4. Serialize to JSON
    //         var jsonContent = new StringContent(
    //             JsonSerializer.Serialize(requestBody),
    //             Encoding.UTF8,
    //             "application/json"
    //         );

    //         // 5. Post to OpenAI
    //         var response = await client.PostAsync("https://api.openai.com/v1/completions", jsonContent);
    //         var responseString = await response.Content.ReadAsStringAsync();

    //         // 6. Check for success
    //         if (!response.IsSuccessStatusCode)
    //         {
    //             return $"Error calling OpenAI API: {response.StatusCode}, {responseString}";
    //         }

    //         // 7. Parse the response
    //         using var doc = JsonDocument.Parse(responseString);
    //         var root = doc.RootElement;
    //         var choices = root.GetProperty("choices");
    //         var text = choices[0].GetProperty("text").GetString();

    //         return text.Trim();
    //     }
    // }


    // /// <summary>
    // /// DTO for plan generation request.
    // /// </summary>
    // public class PlanRequestDto
    // {
    //     public int StudentId { get; set; }
    //     public double CurrentGPA { get; set; }
    //     public double DesiredGPA { get; set; }
    //     public int TotalCreditsNeeded { get; set; }
    // }
    }
}

