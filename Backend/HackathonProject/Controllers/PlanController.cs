using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using HackathonProject.Models;
using System.Net.Http.Headers;

namespace HackathonProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlanController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public PlanController(DataContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// POST: api/plan/generate
        /// Generates an academic plan using OpenAI Chat Completion (gpt-3.5-turbo)
        /// by returning a detailed JSON structure.
        /// </summary>
        [HttpPost("generate")]
        public async Task<IActionResult> GeneratePlan([FromBody] PlanRequestDto dto)
        {
            // 1. Verify the student exists
            var student = await _context.Students.FindAsync(dto.StudentId);
            if (student == null)
            {
                return NotFound($"No student found with ID = {dto.StudentId}");
            }

            // 2. Optionally, get the student's course details from DB
            var courses = await _context.Courses
                .Where(c => c.StudentId == dto.StudentId)
                .ToListAsync();
            var coursesInfo = courses.Any()
                ? string.Join("\n", courses.Select(c => $"Course: {c.CourseName} (Semester: {c.Semester}, Credits: {c.Credits}, Current Grade: {c.Grade})"))
                : "No course information available.";

            // 3. Build the enhanced prompt for a JSON-formatted response
            var prompt = $@"
Return your response as valid JSON. Do NOT include code fences (e.g., ```).

Use this structure exactly:

{{
  ""courses"": [
    {{
      ""courseName"": ""string"",
      ""recommendation"": ""string""
    }}
  ],
  ""gradeBreakdown"": [
    {{
      ""courseName"": ""string"",
      ""tips"": ""string""
    }}
  ],
  ""timeManagement"": {{
    ""recommendedSchedule"": {{
      ""Monday"": ""string"",
      ""Tuesday"": ""string"",
      ""Wednesday"": ""string"",
      ""Thursday"": ""string"",
      ""Friday"": ""string"",
      ""Saturday"": ""string"",
      ""Sunday"": ""string""
    }}
  }},
  ""studyMethods"": {{
    ""recommendedTechniques"": [
      {{
        ""name"": ""string"",
        ""description"": ""string""
      }}
    ]
  }},
  ""resources"": {{
    ""campus"": ""string"",
    ""online"": ""string""
  }},
  ""motivation"": ""string"",
  ""innovativeSuggestions"": ""string""
}}

Student Information:
  - StudentId: {dto.StudentId}
  - Current GPA: {dto.CurrentGPA}
  - Desired GPA: {dto.DesiredGPA}
  - Credits Remaining: {dto.TotalCreditsNeeded}
  - Strengths: {dto.Strengths}
  - Weaknesses: {dto.Weaknesses}

Courses Data:
{coursesInfo}

Provide a thorough, detailed academic plan in the JSON structure above.
";

            // 4. Call OpenAI chat completion with the enhanced prompt
            string openAiResponse = await CallOpenAIChatAsync(prompt);

            // 5. Clean up the response by removing any accidental code fences
            string cleanedResponse = openAiResponse;
            if (cleanedResponse.StartsWith("```json"))
            {
                cleanedResponse = cleanedResponse.Substring("```json".Length);
            }
            if (cleanedResponse.EndsWith("```"))
            {
                cleanedResponse = cleanedResponse.Substring(0, cleanedResponse.LastIndexOf("```"));
            }
            cleanedResponse = cleanedResponse.Trim();

            // 6. Attempt to parse the cleaned response as JSON
            try
            {
                var parsedJson = JsonSerializer.Deserialize<JsonElement>(cleanedResponse);
                // Return the parsed JSON object directly
                return Ok(parsedJson);
            }
            catch (Exception ex)
            {
                // If parsing fails, return an error message
                return BadRequest(new { error = "Unable to parse JSON. " + ex.Message });
            }
        }

        /// <summary>
        /// Helper method to call OpenAI Chat Completion using gpt-3.5-turbo.
        /// </summary>
        /// <param name="userPrompt">The complete prompt including instructions.</param>
        /// <returns>A string containing the assistant's reply (expected as JSON).</returns>
        private async Task<string> CallOpenAIChatAsync(string userPrompt)
        {
            // Retrieve API key from configuration
            string apiKey = _config["OpenAI:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                return "OpenAI API key not configured.";
            }

            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "system", content = "You are a highly knowledgeable academic advisor who provides extremely detailed and actionable academic plans." },
                    new { role = "user", content = userPrompt }
                },
                max_tokens = 800, // Increase if more detail is needed
                temperature = 0.7
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", jsonContent);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return $"Error calling OpenAI API: {response.StatusCode}, {responseString}";
            }

            try
            {
                using var doc = JsonDocument.Parse(responseString);
                var choices = doc.RootElement.GetProperty("choices");
                var content = choices[0].GetProperty("message").GetProperty("content").GetString();
                return content.Trim();
            }
            catch (Exception ex)
            {
                return $"Error parsing OpenAI response: {ex.Message}";
            }
        }
    }

    /// <summary>
    /// DTO for plan generation request.
    /// </summary>
    public class PlanRequestDto
    {
        public int StudentId { get; set; }
        public double CurrentGPA { get; set; }
        public double DesiredGPA { get; set; }
        public int TotalCreditsNeeded { get; set; }
        public string Strengths { get; set; }
        public string Weaknesses { get; set; }
    }
}
